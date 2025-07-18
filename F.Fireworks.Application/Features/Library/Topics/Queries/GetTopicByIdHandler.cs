using Ardalis.Result;
using F.Fireworks.Application.Contracts.Persistence;
using F.Fireworks.Application.DTOs.Courses;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace F.Fireworks.Application.Features.Library.Topics.Queries;

public class GetTopicByIdHandler(IApplicationDbContext context)
    : IRequestHandler<GetTopicByIdQuery, Result<LibraryTopicDto>>
{
    public async Task<Result<LibraryTopicDto>> Handle(GetTopicByIdQuery request, CancellationToken cancellationToken)
    {
        var topic = await context.LibraryTopics.AsNoTracking().Where(t => t.Id == request.Id)
            .ProjectToType<LibraryTopicDto>().FirstOrDefaultAsync(cancellationToken);
        return topic is not null ? Result.Success(topic) : Result.NotFound();
    }
}