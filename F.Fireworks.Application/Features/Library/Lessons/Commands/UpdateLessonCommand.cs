using Ardalis.Result;
using F.Fireworks.Shared.Enums;
using MediatR;

namespace F.Fireworks.Application.Features.Library.Lessons.Commands;

public record UpdateLessonCommand(
    Guid Id,
    string Title,
    string? Content,
    string? VideoUrl,
    int DurationInMinutes,
    int SortOrder,
    bool IsTrial,
    LessonStatus Status,
    Guid LibraryTopicId) : IRequest<Result<Guid>>;