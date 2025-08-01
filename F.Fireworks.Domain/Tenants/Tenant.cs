using F.Fireworks.Domain.Common;
using F.Fireworks.Domain.Identity;
using F.Fireworks.Domain.Subscriptions;
using F.Fireworks.Shared.Enums;

namespace F.Fireworks.Domain.Tenants;

public class Tenant : IEntity<Guid>, IAuditable, ISoftDeletable
{
    public string Name { get; set; } = null!;
    public TenantType Type { get; set; }
    public bool IsActive { get; set; }

    /// <summary>
    ///     存储特定于租户类型的自定义属性 (JSON格式)
    /// </summary>
    public string? Attributes { get; set; }

    public Guid? PlanId { get; set; }
    public virtual Plan? Plan { get; set; }

    public virtual ICollection<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();

    // --- 审计字段 ---
    public Guid? CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
    public Guid? LastModifiedBy { get; set; }
    public DateTime? LastModifiedOn { get; set; }
    public Guid Id { get; set; }


    // --- 软删除字段 ---
    public Guid? DeletedBy { get; set; }
    public DateTime? DeletedOn { get; set; }
    public bool IsDeleted { get; set; }
}