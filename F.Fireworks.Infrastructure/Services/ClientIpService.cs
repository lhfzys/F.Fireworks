using F.Fireworks.Application.Contracts.Services;
using Microsoft.AspNetCore.Http;

namespace F.Fireworks.Infrastructure.Services;

public class ClientIpService(IHttpContextAccessor httpContextAccessor) : IClientIpService
{
    public string GetClientIp()
    {
        var httpContext = httpContextAccessor.HttpContext;
        if (httpContext == null) return "N/A";

        var forwardedFor = httpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
        if (!string.IsNullOrWhiteSpace(forwardedFor))
            return forwardedFor.Split(',', StringSplitOptions.RemoveEmptyEntries).FirstOrDefault() ?? "N/A";

        return httpContext.Connection.RemoteIpAddress?.ToString() ?? "N/A";
    }
}