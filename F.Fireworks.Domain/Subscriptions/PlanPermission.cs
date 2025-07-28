using F.Fireworks.Domain.Permissions;

namespace F.Fireworks.Domain.Subscriptions;

public class PlanPermission
{
    public Guid PlanId { get; set; }
    public Guid PermissionId { get; set; }

    public virtual Plan Plan { get; set; } = null!;
    public virtual Permission Permission { get; set; } = null!;
}