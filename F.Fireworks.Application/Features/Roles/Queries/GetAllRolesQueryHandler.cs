using Ardalis.Result;
using F.Fireworks.Application.Common.Extensions;
using F.Fireworks.Application.Contracts.Persistence;
using F.Fireworks.Application.Contracts.Services;
using F.Fireworks.Application.DTOs.Common;
using F.Fireworks.Application.DTOs.Roles;
using F.Fireworks.Domain.Constants;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace F.Fireworks.Application.Features.Roles.Queries;

public class GetAllRolesQueryHandler(ICurrentUserService currentUser, IApplicationDbContext context)
    : IRequestHandler<GetAllRolesQuery, Result<PaginatedList<RoleDto>>>
{
    public async Task<Result<PaginatedList<RoleDto>>> Handle(GetAllRolesQuery request,
        CancellationToken cancellationToken)
    {
        var query = context.Roles.Include(r => r.Tenant).AsNoTracking();
        if (!currentUser.IsInRole(RoleConstants.SuperAdmin))
            query = query.Where(r => r.TenantId == currentUser.TenantId);

        var paginatedResult = await query
            .ApplyFiltering(request.Filter)
            .ApplySort(request.Filter.SortField, request.Filter.SortOrder)
            .Select(r => new RoleDto(
                r.Id,
                r.Name,
                r.Description,
                r.CreatedOn,
                r.TenantId,
                r.Tenant.Name
            ))
            .ToPaginatedListAsync(request.Filter.PageNumber, request.Filter.PageSize, cancellationToken);
        return Result<PaginatedList<RoleDto>>.Success(paginatedResult);
    }
}