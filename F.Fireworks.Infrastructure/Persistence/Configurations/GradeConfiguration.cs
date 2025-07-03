using F.Fireworks.Domain.Courses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace F.Fireworks.Infrastructure.Persistence.Configurations;

public class GradeConfiguration : IEntityTypeConfiguration<Grade>
{
    public void Configure(EntityTypeBuilder<Grade> builder)
    {
        builder.ToTable("Grades");
        builder.HasKey(g => g.Id);
        builder.Property(g => g.Name).IsRequired().HasMaxLength(100);

        builder.HasMany(g => g.Topics)
            .WithOne(t => t.Grade)
            .HasForeignKey(t => t.GradeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}