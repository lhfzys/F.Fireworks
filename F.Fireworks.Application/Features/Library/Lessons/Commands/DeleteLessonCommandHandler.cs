using Ardalis.Result;
using F.Fireworks.Application.Contracts.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace F.Fireworks.Application.Features.Library.Lessons.Commands;

public class DeleteLessonCommandHandler(IApplicationDbContext context) : IRequestHandler<DeleteLessonCommand, Result>
{
    public async Task<Result> Handle(DeleteLessonCommand request, CancellationToken cancellationToken)
    {
        var lesson = await context.LibraryLessons
            .FirstOrDefaultAsync(g => g.Id == request.Id, cancellationToken);

        if (lesson == null)
            return Result.NotFound("专题不存在");

        context.LibraryLessons.Remove(lesson);
        await context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}