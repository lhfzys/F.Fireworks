using Ardalis.Result;
using MediatR;

namespace F.Fireworks.Application.Features.Library.Lessons.Commands;

public record CreateLessonCommand(
    string Title,
    string? Content,
    string? VideoUrl,
    int DurationInMinutes,
    int SortOrder,
    bool IsTrial,
    Guid LibraryTopicId) : IRequest<Result<Guid>>;