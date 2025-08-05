namespace F.Fireworks.Application.DTOs.Roles;

public record RoleDto(
    Guid Id,
    string? Name,
    string? Description,
    DateTime CreatedOn,
    Guid TenantId,
    string? TenantName,
    bool IsProtected);

public record RoleDetailsDto(
    Guid Id,
    string? Name,
    string? Description,
    DateTime CreatedOn,
    Guid TenantId,
    string? TenantName,
    List<Guid> PermissionIds,
    bool IsProtected);