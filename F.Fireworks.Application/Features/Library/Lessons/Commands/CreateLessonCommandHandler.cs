using Ardalis.Result;
using F.Fireworks.Application.Contracts.Persistence;
using F.Fireworks.Domain.Courses;
using MediatR;

namespace F.Fireworks.Application.Features.Library.Lessons.Commands;

public class CreateLessonCommandHandler(IApplicationDbContext context)
    : IRequestHandler<CreateLessonCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateLessonCommand request, CancellationToken cancellationToken)
    {
        var lesson = new LibraryLesson
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            SortOrder = request.SortOrder,
            LibraryTopicId = request.LibraryTopicId,
            IsTrial = request.IsTrial,
            DurationInMinutes = request.DurationInMinutes,
            Content = request.Content,
            VideoUrl = request.VideoUrl
        };

        await context.LibraryLessons.AddAsync(lesson, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success(lesson.Id);
    }
}