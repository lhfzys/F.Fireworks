using F.Fireworks.Domain.Common;
using F.Fireworks.Domain.Identity;

namespace F.Fireworks.Domain.Logging;

public class UserLoginLog : IEntity<Guid>
{
    public Guid? UserId { get; set; }
    public Guid? TenantId { get; set; }
    public string IpAddress { get; set; } = null!;
    public string? Device { get; set; }
    public string? Location { get; set; }
    public string? UserAgent { get; set; }
    public DateTime LoginTime { get; set; } = DateTime.UtcNow;
    public bool WasSuccessful { get; set; }
    public string? FailureReason { get; set; }

    // --- 导航属性 ---
    public virtual ApplicationUser? User { get; set; } =

        null!;

    public Guid Id { get; set; }
}