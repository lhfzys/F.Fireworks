using Ardalis.Result;
using F.Fireworks.Application.Contracts.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace F.Fireworks.Application.Features.Library.Lessons.Commands;

public class UpdateLessonCommandHandler(IApplicationDbContext context)
    : IRequestHandler<UpdateLessonCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(UpdateLessonCommand request, CancellationToken cancellationToken)
    {
        var lesson = await context.LibraryLessons
            .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);
        if (lesson == null)
            return Result.NotFound("课节不存在");
        lesson.Title = request.Title;
        lesson.Content = request.Content;
        lesson.VideoUrl = request.VideoUrl;
        lesson.DurationInMinutes = request.DurationInMinutes;
        lesson.SortOrder = request.SortOrder;
        lesson.IsTrial = request.IsTrial;
        lesson.LibraryTopicId = request.LibraryTopicId;
        lesson.Status = request.Status;
        await context.SaveChangesAsync(cancellationToken);
        return Result.Success(lesson.Id);
    }
}