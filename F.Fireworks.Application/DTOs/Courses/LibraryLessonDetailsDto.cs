namespace F.Fireworks.Application.DTOs.Courses;

public record LibraryLessonDetailsDto : LibraryLessonDto
{
    public string? Content { get; init; }
    public string? VideoUrl { get; init; }
}