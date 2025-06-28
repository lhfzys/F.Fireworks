using F.Fireworks.Shared.Enums;

namespace F.Fireworks.Domain.Permissions;

[AttributeUsage(AttributeTargets.Field)]
public class PermissionAttribute(
    string description,
    PermissionType type,
    int sortOrder,
    string? path = null,
    string? parentCode = null)
    : Attribute
{
    public string Description { get; } = description;
    public PermissionType Type { get; } = type;
    public int SortOrder { get; } = sortOrder;
    public string? Path { get; } = path;
    public string? ParentCode { get; } = parentCode;
}