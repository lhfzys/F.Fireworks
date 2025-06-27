using F.Fireworks.Domain.Permissions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace F.Fireworks.Infrastructure.Persistence.Configurations;

public class RolePermissionConfiguration : IEntityTypeConfiguration<ApplicationRolePermission>
{
    public void Configure(EntityTypeBuilder<ApplicationRolePermission> builder)
    {
        builder.ToTable("RolePermissions");
        builder.HasKey(p => new { p.RoleId, p.PermissionId });

        // 配置与角色的多对一关系
        builder.HasOne(p => p.Role)
            .WithMany(r =>
                r.Permissions) // 注意 ApplicationRole 中需要有 public ICollection<ApplicationRolePermission> Permissions 导航属性
            .HasForeignKey(p => p.RoleId)
            .OnDelete(DeleteBehavior.Cascade);

        // 配置与权限的多对一关系
        builder.HasOne(p => p.Permission)
            .WithMany(p => p.Roles) // 注意 Permission 中需要有 public ICollection<ApplicationRolePermission> Roles 导航属性
            .HasForeignKey(p => p.PermissionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}