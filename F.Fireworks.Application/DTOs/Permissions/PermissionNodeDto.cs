using F.Fireworks.Shared.Enums;

namespace F.Fireworks.Application.DTOs.Permissions;

/// <summary>
///     用于表示权限树节点的DTO
/// </summary>
public record PermissionNodeDto
{
    /// <summary>
    ///     权限ID
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    ///     权限唯一编码
    /// </summary>
    public string Code { get; init; } = string.Empty;

    /// <summary>
    ///     显示名称
    /// </summary>
    public string DisplayName { get; init; } = string.Empty;

    /// <summary>
    ///     权限类型
    /// </summary>
    public PermissionType Type { get; init; }

    /// <summary>
    ///     子节点列表
    /// </summary>
    public List<PermissionNodeDto> Children { get; set; } = new();
}