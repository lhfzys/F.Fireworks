using F.Fireworks.Domain.Common;
using F.Fireworks.Shared.Enums;

namespace F.Fireworks.Domain.Tenants;

public class Tenant : IEntity<Guid>, IAuditable
{
    public string Name { get; set; } = null!;
    public TenantType Type { get; set; }
    public bool IsActive { get; set; }

    /// <summary>
    ///     存储特定于租户类型的自定义属性 (JSON格式)
    /// </summary>
    public string? Attributes { get; set; }

    // --- 审计字段 ---
    public Guid? CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
    public Guid? LastModifiedBy { get; set; }
    public DateTime? LastModifiedOn { get; set; }
    public Guid Id { get; set; }
}