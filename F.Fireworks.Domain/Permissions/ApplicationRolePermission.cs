using F.Fireworks.Domain.Identity;

namespace F.Fireworks.Domain.Permissions;

public class ApplicationRolePermission
{
    public Guid RoleId { get; set; }
    public Guid PermissionId { get; set; }

    // --- 导航属性 ---
    public virtual ApplicationRole Role { get; set; } = null!;
    public virtual Permission Permission { get; set; } = null!;
}