using F.Fireworks.Domain.Common;
using F.Fireworks.Domain.Tenants;

namespace F.Fireworks.Domain.Subscriptions;

public class Plan : IEntity<Guid>, IAuditable
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public bool IsActive { get; set; }

    // 导航属性
    public virtual ICollection<PlanPermission> Permissions { get; set; } = new List<PlanPermission>();
    public virtual ICollection<Tenant> Tenants { get; set; } = new List<Tenant>();

    // 审计字段
    public Guid? CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
    public Guid? LastModifiedBy { get; set; }
    public DateTime? LastModifiedOn { get; set; }
    public Guid Id { get; set; }
}