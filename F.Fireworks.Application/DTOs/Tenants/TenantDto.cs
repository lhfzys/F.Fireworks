using F.Fireworks.Shared.Enums;

namespace F.Fireworks.Application.DTOs.Tenants;

public record TenantDto(Guid Id, string Name, TenantType Type, bool IsActive);