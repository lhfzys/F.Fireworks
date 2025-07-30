using Ardalis.Result;
using F.Fireworks.Application.Contracts.Persistence;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace F.Fireworks.Application.Features.Authentication.Commands;

public class LogoutCommandHandler(IApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
    : IRequestHandler<LogoutCommand, Result>
{
    public async Task<Result> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        var httpContext = httpContextAccessor.HttpContext;
        if (httpContext is null) return Result.Success();
        // 1. 从 Cookie 中读取 Refresh Token
        var refreshTokenString = httpContext.Request.Cookies["refreshToken"];
        // 2. 在数据库中吊销 Token（如果存在）
        if (!string.IsNullOrEmpty(refreshTokenString))
        {
            var storedToken = await context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.Token == refreshTokenString, cancellationToken);

            if (storedToken is { IsActive: true })
            {
                storedToken.RevokedOn = DateTime.UtcNow;
                await context.SaveChangesAsync(cancellationToken);
            }
        }

        // 3. 指示浏览器删除 Cookie
        //    通过发送一个同名、空值且立即过期的 Cookie 来实现
        httpContext.Response.Cookies.Delete("refreshToken");
        await context.SaveChangesAsync(cancellationToken);

        return Result.SuccessWithMessage("Successfully logged out.");
    }
}