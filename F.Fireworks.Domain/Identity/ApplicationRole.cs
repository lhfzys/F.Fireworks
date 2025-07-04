using F.Fireworks.Domain.Common;
using F.Fireworks.Domain.Permissions;
using Microsoft.AspNetCore.Identity;

namespace F.Fireworks.Domain.Identity;

public class ApplicationRole : IdentityRole<Guid>, IAuditable
{
    // --- 导航属性 ---
    public ICollection<ApplicationRolePermission> Permissions = new List<ApplicationRolePermission>();
    public string? Description { get; set; }

    /// <summary>
    ///     角色所属的租户ID
    /// </summary>
    public Guid TenantId { get; set; }

    // --- 审计字段 ---
    public Guid? CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
    public Guid? LastModifiedBy { get; set; }
    public DateTime? LastModifiedOn { get; set; }
}