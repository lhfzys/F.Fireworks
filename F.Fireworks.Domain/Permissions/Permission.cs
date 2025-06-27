using F.Fireworks.Domain.Common;
using F.Fireworks.Shared.Enums;

namespace F.Fireworks.Domain.Permissions;

public class Permission : IEntity<Guid>
{
    public string Code { get; set; } = null!;
    public string DisplayName { get; set; } = null!;
    public PermissionType Type { get; set; }
    public string? Description { get; set; }
    public int SortOrder { get; set; }

    // --- 用于菜单/目录的属性 ---
    public Guid? ParentId { get; set; }
    public string? Path { get; set; }
    public string? Icon { get; set; }

    // --- 导航属性 ---
    public virtual Permission? Parent { get; set; }
    public virtual ICollection<Permission> Children { get; set; } = new List<Permission>();
    public virtual ICollection<ApplicationRolePermission> Roles { get; set; } = new List<ApplicationRolePermission>();
    public Guid Id { get; set; }
}