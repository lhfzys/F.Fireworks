using System.ComponentModel.DataAnnotations.Schema;
using F.Fireworks.Domain.Common;
using F.Fireworks.Domain.Tenants;
using F.Fireworks.Shared.Enums;
using Microsoft.AspNetCore.Identity;

namespace F.Fireworks.Domain.Identity;

public class ApplicationUser : IdentityUser<Guid>, IAuditable, ISoftDeletable
{
    /// <summary>
    ///     用户的业务状态
    /// </summary>
    public UserStatus Status { get; set; } = UserStatus.Inactive;

    /// <summary>
    ///     关联的租户ID
    /// </summary>
    public Guid TenantId { get; set; }

    /// <summary>
    ///     需要修改密码
    /// </summary>
    public bool MustChangePassword { get; set; }

    // --- 导航属性 ---
    public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();

    [ForeignKey(nameof(TenantId))] public virtual Tenant Tenant { get; set; } = null!;

    // --- 审计字段 ---
    public Guid? CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
    public Guid? LastModifiedBy { get; set; }
    public DateTime? LastModifiedOn { get; set; }

    // --- 软删除字段 ---
    public Guid? DeletedBy { get; set; }
    public DateTime? DeletedOn { get; set; }
    public bool IsDeleted { get; set; }
}