using System.Security.Claims;
using F.Fireworks.Application.Contracts.Services;
using Microsoft.AspNetCore.Http;

namespace F.Fireworks.Infrastructure.Services;

public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    public Guid? TenantId => httpContextAccessor.HttpContext?.User?.FindFirstValue("tenant") is { } id &&
                             Guid.TryParse(id, out var tenantId)
        ? tenantId
        : null;

    public Guid? UserId =>
        httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier) is { } idStr &&
        Guid.TryParse(idStr, out var id)
            ? id
            : null;

    public string? UserName => httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name);

    public string? GetClaim(string claimType)
    {
        return httpContextAccessor.HttpContext?.User?.FindFirstValue(claimType);
    }

    public string? GetIpAddress()
    {
        var httpContext = httpContextAccessor.HttpContext;
        if (httpContext == null) return null;

        var forwardedFor = httpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
        if (!string.IsNullOrWhiteSpace(forwardedFor))
            return forwardedFor.Split(',', StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();

        return httpContext.Connection.RemoteIpAddress?.ToString();
    }

    public string? GetUserAgent()
    {
        return httpContextAccessor.HttpContext?.Request.Headers.UserAgent.ToString();
    }

    public bool IsInRole(string roleName)
    {
        return httpContextAccessor.HttpContext?.User?.IsInRole(roleName) ?? false;
    }
}