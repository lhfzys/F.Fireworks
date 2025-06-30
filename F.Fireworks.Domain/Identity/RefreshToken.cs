using F.Fireworks.Domain.Common;

namespace F.Fireworks.Domain.Identity;

public class RefreshToken : IEntity<Guid>
{
    public string Token { get; set; } = null!;
    public DateTime Expires { get; set; }
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    public string CreatedByIp { get; set; } = null!;

    public string? UserAgent { get; set; }
    public string? Jti { get; set; }
    public DateTime? RevokedOn { get; set; }
    public string? ReplacedByToken { get; set; }

    /// <summary>
    ///     关联的租户ID，用于数据分区和简化查询
    /// </summary>
    public Guid TenantId { get; set; }

    // --- 计算属性 ---
    public bool IsExpired => DateTime.UtcNow >= Expires;
    public bool IsRevoked => RevokedOn != null;
    public bool IsActive => !IsRevoked && !IsExpired;

    // --- 外键和导航属性 ---
    public Guid UserId { get; set; }
    public virtual ApplicationUser User { get; set; } = null!;
    public Guid Id { get; set; }
}