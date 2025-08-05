using Ardalis.Result;
using F.Fireworks.Application.Common.Extensions;
using F.Fireworks.Application.Contracts.Services;
using F.Fireworks.Application.DTOs.Common;
using F.Fireworks.Application.DTOs.Users;
using F.Fireworks.Domain.Constants;
using F.Fireworks.Domain.Identity;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace F.Fireworks.Application.Features.Users.Queries;

public class GetUsersQueryHandler(ICurrentUserService currentUser, UserManager<ApplicationUser> userManager)
    : IRequestHandler<GetUsersQuery, Result<PaginatedList<UserDto>>>
{
    public async Task<Result<PaginatedList<UserDto>>> Handle(GetUsersQuery request,
        CancellationToken cancellationToken)
    {
        var query = userManager.Users.Include(u => u.Tenant).AsNoTracking();

        if (!currentUser.IsInRole(RoleConstants.SuperAdmin))
            query = query.Where(u => u.TenantId == currentUser.TenantId);
        else if (request.Filter.TenantId.HasValue)
            query = query.Where(u => u.TenantId == request.Filter.TenantId.Value);

        TypeAdapterConfig<ApplicationUser, UserDto>.NewConfig()
            .Map(dest => dest.TenantName, src => src.Tenant.Name);

        var paginatedResult = await query
            .ApplyFiltering(request.Filter)
            .ApplySort(request.Filter.SortField, request.Filter.SortOrder)
            .ProjectToType<UserDto>()
            .ToPaginatedListAsync(request.Filter.PageNumber, request.Filter.PageSize, cancellationToken);
        return Result<PaginatedList<UserDto>>.Success(paginatedResult);
    }
}