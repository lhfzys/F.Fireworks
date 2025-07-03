using F.Fireworks.Shared.Enums;

namespace F.Fireworks.Application.DTOs.Courses;

public record LibraryLessonDto
{
    public Guid Id { get; init; }
    public string Title { get; init; } = null!;
    public int SortOrder { get; init; }
    public bool IsTrial { get; init; }
    public LessonStatus Status { get; init; }
    public int DurationInMinutes { get; init; }
    public Guid LibraryTopicId { get; init; }
    public string TopicName { get; init; } = null!;
}