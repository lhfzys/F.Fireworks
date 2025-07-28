using F.Fireworks.Domain.Subscriptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace F.Fireworks.Infrastructure.Persistence.Configurations;

public class PlanPermissionConfiguration : IEntityTypeConfiguration<PlanPermission>
{
    public void Configure(EntityTypeBuilder<PlanPermission> builder)
    {
        builder.ToTable("PlanPermissions");
        builder.HasKey(pp => new { pp.PlanId, pp.PermissionId });
        builder.HasOne(pp => pp.Plan)
            .WithMany(p => p.Permissions)
            .HasForeignKey(pp => pp.PlanId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}