using F.Fireworks.Domain.Courses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace F.Fireworks.Infrastructure.Persistence.Configurations;

public class LibraryTopicConfiguration : IEntityTypeConfiguration<LibraryTopic>
{
    public void Configure(EntityTypeBuilder<LibraryTopic> builder)
    {
        builder.ToTable("LibraryTopics");
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Name).IsRequired().HasMaxLength(200);

        builder.HasMany(t => t.Lessons)
            .WithOne(l => l.Topic)
            .HasForeignKey(l => l.LibraryTopicId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}