using Ardalis.Result;
using F.Fireworks.Application.Contracts.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace F.Fireworks.Application.Features.Library.Topics.Commands;

public class UpdateTopicCommandHandler(IApplicationDbContext context)
    : IRequestHandler<UpdateTopicCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(UpdateTopicCommand request, CancellationToken cancellationToken)
    {
        var topic = await context.LibraryTopics
            .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);
        if (topic == null)
            return Result.NotFound("专题不存在");
        topic.Name = request.Name;
        topic.SortOrder = request.SortOrder;
        topic.Description = request.Description;
        topic.CoverImageUrl = request.CoverImageUrl;
        await context.SaveChangesAsync(cancellationToken);
        return Result.Success(topic.Id);
    }
}