using Ardalis.Result;
using F.Fireworks.Application.Common.Extensions;
using F.Fireworks.Application.Contracts.Persistence;
using F.Fireworks.Application.DTOs.Common;
using F.Fireworks.Application.DTOs.Subscriptions;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace F.Fireworks.Application.Features.Plans.Queries;

public class GetPlansQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetPlansQuery, Result<PaginatedList<PlanDto>>>
{
    public async Task<Result<PaginatedList<PlanDto>>> Handle(GetPlansQuery request, CancellationToken cancellationToken)
    {
        var paginatedResult = await context.Plans
            .AsNoTracking()
            .ApplyFiltering(request.Filter)
            .ProjectToType<PlanDto>()
            .ToPaginatedListAsync(request.Filter.PageNumber, request.Filter.PageSize, cancellationToken);
        return Result<Result<PaginatedList<PlanDto>>>.Success(paginatedResult);
    }
}