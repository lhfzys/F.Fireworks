using F.Fireworks.Shared.Enums;

namespace F.Fireworks.Application.DTOs.Account;

public record UserProfileDto
{
    public Guid Id { get; init; }
    public string? UserName { get; init; }
    public UserStatus Status { get; init; }

    public TenantInfo? Tenant { get; init; }

    public List<string> Roles { get; init; } = [];
    public List<string> Permissions { get; init; } = [];
}

public record TenantInfo(Guid Id, string Name);