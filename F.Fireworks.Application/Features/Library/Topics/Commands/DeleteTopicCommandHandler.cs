using Ardalis.Result;
using F.Fireworks.Application.Contracts.Persistence;
using F.Fireworks.Application.Features.Library.Topics.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace F.Fireworks.Application.Features.Library.Grades.Commands;

public class DeleteTopicCommandHandler(IApplicationDbContext context) : IRequestHandler<DeleteTopicCommand, Result>
{
    public async Task<Result> Handle(DeleteTopicCommand request, CancellationToken cancellationToken)
    {
        var topic = await context.LibraryTopics
            .Include(g => g.Lessons)
            .FirstOrDefaultAsync(g => g.Id == request.Id, cancellationToken);

        if (topic == null)
            return Result.NotFound("专题不存在");

        if (topic.Lessons.Count != 0) return Result.Error("该年级下有多个小节关联，禁止删除");

        context.LibraryTopics.Remove(topic);
        await context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}