namespace F.Fireworks.Application.DTOs.Courses;

public record LibraryTopicDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = null!;
    public string? Description { get; init; }
    public string? CoverImageUrl { get; init; }
    public int SortOrder { get; init; }

    // 包含父级年级的信息
    public Guid GradeId { get; init; }
    public string GradeName { get; init; } = null!;
}