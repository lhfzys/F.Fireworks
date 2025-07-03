using Ardalis.Result;
using F.Fireworks.Application.Common.Extensions;
using F.Fireworks.Application.Contracts.Persistence;
using F.Fireworks.Application.DTOs.Common;
using F.Fireworks.Application.DTOs.Courses;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace F.Fireworks.Application.Features.Library.Topics.Queries;

public class GetTopicsQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetTopicsQuery, Result<PaginatedList<LibraryTopicDto>>>
{
    public async Task<Result<PaginatedList<LibraryTopicDto>>> Handle(GetTopicsQuery request,
        CancellationToken cancellationToken)
    {
        var query = context.LibraryTopics.Include(t => t.Grade).AsNoTracking();

        var paginatedResult = await query
            .ApplyFiltering(request.Filter)
            .ApplySort(request.Filter.SortField, request.Filter.SortOrder)
            .ProjectToType<LibraryTopicDto>()
            .ToPaginatedListAsync(request.Filter.PageNumber, request.Filter.PageSize, cancellationToken);

        return Result<PaginatedList<LibraryTopicDto>>.Success(paginatedResult);
    }
}