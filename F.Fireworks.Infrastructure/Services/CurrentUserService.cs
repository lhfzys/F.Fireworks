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
}