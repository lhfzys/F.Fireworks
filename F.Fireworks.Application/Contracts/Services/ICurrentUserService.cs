namespace F.Fireworks.Application.Contracts.Services;

public interface ICurrentUserService
{
    Guid? UserId { get; }
    string? UserName { get; }
    Guid? TenantId { get; }

    bool IsInRole(string roleName);

    string? GetIpAddress();
    string? GetUserAgent();
    string? GetClaim(string claimType);
}