using Ardalis.Result;
using F.Fireworks.Application.Contracts.Persistence;
using F.Fireworks.Domain.Courses;
using MediatR;

namespace F.Fireworks.Application.Features.Library.Topics.Commands;

public class CreateTopicCommandHandler(IApplicationDbContext context)
    : IRequestHandler<CreateTopicCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateTopicCommand request, CancellationToken cancellationToken)
    {
        var libraryTopic = new LibraryTopic
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            SortOrder = request.SortOrder,
            GradeId = request.GradeId,

            CoverImageUrl = request.CoverImageUrl,
            Description = request.Description
        };

        await context.LibraryTopics.AddAsync(libraryTopic, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success(libraryTopic.Id);
    }
}