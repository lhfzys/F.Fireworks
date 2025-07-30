using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using Ardalis.Result;
using F.Fireworks.Application.Contracts.Identity;
using F.Fireworks.Application.Contracts.Persistence;
using F.Fireworks.Application.Contracts.Services;
using F.Fireworks.Application.DTOs.Authentication;
using F.Fireworks.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace F.Fireworks.Application.Features.Authentication.Commands;

public class RefreshTokenCommandHandler(
    IApplicationDbContext context,
    UserManager<ApplicationUser> userManager,
    ITokenService tokenService,
    IClientIpService clientIpService,
    ICurrentUserService currentUserService,
    IHttpContextAccessor httpContextAccessor)
    : IRequestHandler<RefreshTokenCommand, Result<LoginResponse>>
{
    public async Task<Result<LoginResponse>> Handle(RefreshTokenCommand request,
        CancellationToken cancellationToken)
    {
        var httpContext = httpContextAccessor.HttpContext;
        if (httpContext is null)
            return Result.Error("系统暂时无法处理您的请求，请刷新后重试。");

        // 1. 从 HttpOnly Cookie 中读取旧的 Refresh Token
        var oldRefreshTokenString = httpContext.Request.Cookies["refreshToken"];
        if (string.IsNullOrEmpty(oldRefreshTokenString)) return Result<LoginResponse>.Unauthorized();

        // 2. 从数据库查找这个 Refresh Token
        var storedToken = await context.RefreshTokens
            .Include(rt => rt.User)
            .SingleOrDefaultAsync(rt => rt.Token == oldRefreshTokenString, cancellationToken);

        // 3. 执行安全校验
        if (storedToken is null || storedToken.IsRevoked || storedToken.IsExpired)
            return Result<LoginResponse>.Unauthorized();

        // --- 4. 执行令牌旋转 (Token Rotation) ---
        // a. 将旧令牌标记为已吊销
        storedToken.RevokedOn = DateTime.UtcNow;
        // b. 生成新的 Access Token
        var user = storedToken.User;
        var roles = await userManager.GetRolesAsync(user);
        var newAccessToken = tokenService.CreateToken(user, roles);
        var newJti = new JwtSecurityTokenHandler().ReadJwtToken(newAccessToken).Id;
        // c. 生成并持久化新的 Refresh Token
        var newRefreshTokenString = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        var newRefreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = storedToken.UserId,
            TenantId = storedToken.TenantId,
            Token = newRefreshTokenString,
            Jti = newJti,
            Expires = DateTime.UtcNow.AddDays(30),
            CreatedByIp = clientIpService.GetClientIp(),
            UserAgent = currentUserService.GetUserAgent()
        };
        await context.RefreshTokens.AddAsync(newRefreshToken, cancellationToken);
        // d. 将新的 Refresh Token 写入 HttpOnly Cookie
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = newRefreshToken.Expires
        };
        httpContext.Response.Cookies.Append("refreshToken", newRefreshToken.Token, cookieOptions);

        // 5. 保存数据库更改
        await context.SaveChangesAsync(cancellationToken);

        // 6. 返回只包含新 Access Token 的响应
        return Result<LoginResponse>.Success(new LoginResponse(newAccessToken));
    }
}