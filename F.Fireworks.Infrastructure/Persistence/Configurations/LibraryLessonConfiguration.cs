using F.Fireworks.Domain.Courses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace F.Fireworks.Infrastructure.Persistence.Configurations;

public class LibraryLessonConfiguration : IEntityTypeConfiguration<LibraryLesson>
{
    public void Configure(EntityTypeBuilder<LibraryLesson> builder)
    {
        builder.ToTable("LibraryLessons");
        builder.HasKey(l => l.Id);
        builder.Property(l => l.Title).IsRequired().HasMaxLength(200);
    }
}