using F.Fireworks.Shared.Enums;

namespace F.Fireworks.Domain.Permissions;

[AttributeUsage(AttributeTargets.Field)]
public class PermissionAttribute(
    string description,
    PermissionType type,
    int sortOrder,
    bool isTenantPermission,
    string? path = null,
    string? parentCode = null)
    : Attribute
{
    public string Description { get; } = description;
    public PermissionType Type { get; } = type;
    public int SortOrder { get; } = sortOrder;
    public bool IsTenantPermission { get; } = isTenantPermission;
    public string? Path { get; } = path;
    public string? ParentCode { get; } = parentCode;
}