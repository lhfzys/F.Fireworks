using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using Ardalis.Result;
using F.Fireworks.Application.Contracts.Identity;
using F.Fireworks.Application.Contracts.Persistence;
using F.Fireworks.Application.Contracts.Services;
using F.Fireworks.Application.DTOs.Authentication;
using F.Fireworks.Domain.Constants;
using F.Fireworks.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace F.Fireworks.Application.Features.Authentication.Commands;

public class AdminLoginCommandHandler(
    IApplicationDbContext context,
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager,
    ILoginLogService loginLogService,
    ITokenService tokenService,
    IClientIpService clientIpService,
    ICurrentUserService currentUserService,
    IHttpContextAccessor httpContextAccessor)
    : IRequestHandler<AdminLoginCommand, Result<LoginResponse>>
{
    public async Task<Result<LoginResponse>> Handle(AdminLoginCommand request, CancellationToken cancellationToken)
    {
        var normalizedIdentifier = userManager.NormalizeName(request.Identifier);
        var user = await userManager.Users.FirstOrDefaultAsync(u =>
                u.NormalizedUserName == normalizedIdentifier || u.NormalizedEmail == normalizedIdentifier,
            cancellationToken);

        if (user is null || !await userManager.IsInRoleAsync(user, RoleConstants.SuperAdmin))
        {
            await loginLogService.LogAsync(user?.Id, user?.TenantId ?? Guid.Empty, request.Identifier, false,
                "AdminLoginFailed", cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
            return Result<LoginResponse>.Unauthorized("用户名或密码错误。");
        }

        var result = await signInManager.CheckPasswordSignInAsync(user, request.Password, true);

        if (result.IsLockedOut)
        {
            await loginLogService.LogAsync(user.Id, user.TenantId, request.Identifier, false, "UserLockedOut",
                cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
            return Result<LoginResponse>.Error("您的账户已被锁定，请 15 分钟后再试。");
        }

        if (!result.Succeeded)
        {
            await loginLogService.LogAsync(user.Id, user.TenantId, request.Identifier, false, "InvalidCredentials",
                cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
            return Result<LoginResponse>.Unauthorized("用户名或密码错误。");
        }

        await loginLogService.LogAsync(user.Id, user.TenantId,
            request.Identifier, true, "LoginSuccess", cancellationToken);

        var roles = await userManager.GetRolesAsync(user);
        var accessToken = tokenService.CreateToken(user, roles);

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

        await context.SaveChangesAsync(cancellationToken);
        return Result<LoginResponse>.Success(new LoginResponse(accessToken));
    }
}