using F.Fireworks.Domain.Tenants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace F.Fireworks.Infrastructure.Persistence.Configurations;

public class TenantConfiguration : IEntityTypeConfiguration<Tenant>
{
    public void Configure(EntityTypeBuilder<Tenant> builder)
    {
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Name).IsRequired().HasMaxLength(256);
        builder.Property(t => t.Attributes).HasColumnType("jsonb");
        builder.HasOne(t => t.Plan).WithMany(p => p.Tenants).HasForeignKey(t => t.PlanId);
    }
}