using Ardalis.Result;
using F.Fireworks.Application.Common.Extensions;
using F.Fireworks.Application.Contracts.Persistence;
using F.Fireworks.Application.DTOs.Common;
using F.Fireworks.Application.DTOs.Tenants;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace F.Fireworks.Application.Features.Tenants.Queries;

public class GetTenantsQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetTenantsQuery, Result<PaginatedList<TenantDto>>>
{
    public async Task<Result<PaginatedList<TenantDto>>> Handle(GetTenantsQuery request,
        CancellationToken cancellationToken)
    {
        var paginatedResult = await context.Tenants
            .AsNoTracking()
            .ApplyFiltering(request.Filter)
            .ApplySort(request.Filter.SortField, request.Filter.SortOrder)
            .ProjectToType<TenantDto>()
            .ToPaginatedListAsync(request.Filter.PageNumber, request.Filter.PageSize, cancellationToken);
        return Result<PaginatedList<TenantDto>>.Success(paginatedResult);
    }
}