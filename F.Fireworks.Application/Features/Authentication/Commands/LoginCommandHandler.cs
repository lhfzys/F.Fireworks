using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using Ardalis.Result;
using F.Fireworks.Application.Contracts.Identity;
using F.Fireworks.Application.Contracts.Persistence;
using F.Fireworks.Application.Contracts.Services;
using F.Fireworks.Application.DTOs.Authentication;
using F.Fireworks.Domain.Identity;
using F.Fireworks.Shared.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace F.Fireworks.Application.Features.Authentication.Commands;

public class LoginCommandHandler(
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager,
    ITokenService tokenService,
    ILoginLogService loginLogService,
    IClientIpService clientIpService,
    ICurrentUserService currentUserService,
    IApplicationDbContext context,
    IHttpContextAccessor httpContextAccessor)
    : IRequestHandler<LoginCommand, Result<LoginResponse>>
{
    public async Task<Result<LoginResponse>> Handle(LoginCommand request,
        CancellationToken cancellationToken)
    {
        // 1. 根据租户标识查找租户
        var tenant = await context.Tenants
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Name == request.TenantIdentifier, cancellationToken);
        if (tenant is null) return Result<LoginResponse>.Unauthorized("无效的机构、用户名或密码");

        // 2. 在指定租户下，通过用户名或邮箱查找用户
        var normalizedIdentifier = userManager.NormalizeName(request.Identifier);
        var user = await userManager.Users.FirstOrDefaultAsync(u =>
                u.TenantId == tenant.Id &&
                (u.NormalizedUserName == normalizedIdentifier || u.NormalizedEmail == normalizedIdentifier),
            cancellationToken);

        // 3. 统一处理用户不存在或密码错误的情况，防止时序攻击
        if (user is null)
        {
            await loginLogService.LogAsync(user?.Id, tenant.Id, request.Identifier, false, "InvalidCredentials",
                cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
            return Result<LoginResponse>.Unauthorized("租户、用户名或密码错误。");
        }

        var result = await signInManager.CheckPasswordSignInAsync(user, request.Password, true);
        if (result.IsLockedOut)
        {
            await loginLogService.LogAsync(user.Id, user.TenantId, request.Identifier, false, "UserLockedOut",
                cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
            return Result<LoginResponse>.Error("您的账户已被锁定，请15分钟后再试。");
        }

        if (!result.Succeeded)
        {
            await loginLogService.LogAsync(user.Id, user.TenantId, request.Identifier, false, "InvalidCredentials",
                cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
            return Result<LoginResponse>.Unauthorized("租户、用户名或密码错误。");
        }

        // 4. 检查用户状态
        if (user.Status != UserStatus.Active)
        {
            await loginLogService.LogAsync(user.Id, tenant.Id, request.Identifier, false, "UserNotActive",
                cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
            return Result<LoginResponse>.Unauthorized("该用户未激活或已被禁用");
        }

        // 5. 记录成功日志
        await loginLogService.LogAsync(user.Id, tenant.Id, request.Identifier, true, "LoginSuccess", cancellationToken);

        // 6. 生成 Access Token
        var roles = await userManager.GetRolesAsync(user);
        var accessToken = tokenService.CreateToken(user, roles);

        // 7. 生成并持久化 Refresh Token
        var refreshTokenString = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        var jti = new JwtSecurityTokenHandler().ReadJwtToken(accessToken).Id;

        var refreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            TenantId = user.TenantId,
            Token = refreshTokenString,
            Expires = DateTime.UtcNow.AddDays(30),
            CreatedByIp = clientIpService.GetClientIp(),
            UserAgent = currentUserService.GetUserAgent(),
            Jti = jti
        };
        await context.RefreshTokens.AddAsync(refreshToken, cancellationToken);

        // 8.将 Refresh Token 存入 HttpOnly Cookie
        var response = httpContextAccessor.HttpContext?.Response;
        if (response is null) return Result.Error("系统暂时无法响应您的登录请求，这可能是一个临时网络问题。请您稍后再试。如果此问题反复出现，请联系管理员协助处理。");
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = refreshToken.Expires
        };
        response.Cookies.Append("refreshToken", refreshToken.Token, cookieOptions);

        // 9. 一次性保存所有更改（登录日志 + 新的 Refresh Token）
        await context.SaveChangesAsync(cancellationToken);

        // 10. 返回只包含 AccessToken 的响应
        return Result<LoginResponse>.Success(new LoginResponse(accessToken));
    }
}