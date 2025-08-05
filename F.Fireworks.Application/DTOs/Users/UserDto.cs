using F.Fireworks.Shared.Enums;

namespace F.Fireworks.Application.DTOs.Users;

public record UserDto(
    Guid Id,
    string UserName,
    string Email,
    UserStatus Status,
    DateTime CreatedOn,
    Guid TenantId,
    string? TenantName);