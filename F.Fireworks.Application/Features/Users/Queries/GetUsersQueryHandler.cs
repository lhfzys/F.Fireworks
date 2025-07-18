using Ardalis.Result;
using F.Fireworks.Application.Common.Extensions;
using F.Fireworks.Application.Contracts.Services;
using F.Fireworks.Application.DTOs.Common;
using F.Fireworks.Application.DTOs.Users;
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
        var query = userManager.Users.AsNoTracking();

        if (currentUser.IsInRole("SuperAdmin"))
        {
            // 身份：超级管理员
            // 逻辑：允许按前端传入的 TenantId (如果有) 进行筛选
            if (request.Filter.TenantId.HasValue) query = query.Where(u => u.TenantId == request.Filter.TenantId.Value);
        }
        else
        {
            // 身份：租户用户/管理员
            // 逻辑：强制按其自身的 TenantId 进行筛选，忽略前端传入的任何 TenantId
            var tenantId = currentUser.TenantId;
            if (tenantId is null)
                // 一个非超管用户必须属于一个租户。如果因为某种原因没有，
                // 出于安全考虑，我们返回一个空列表。
                return Result<PaginatedList<UserDto>>.Success(new PaginatedList<UserDto>(new List<UserDto>(), 0, 1,
                    request.Filter.PageSize));

            query = query.Where(u => u.TenantId == tenantId.Value);
        }

        var paginatedResult = await query
            .ApplyFiltering(request.Filter)
            .ApplySort(request.Filter.SortField, request.Filter.SortOrder)
            .ProjectToType<UserDto>()
            .ToPaginatedListAsync(request.Filter.PageNumber, request.Filter.PageSize, cancellationToken); // 步骤 4: 分页
        return Result<PaginatedList<UserDto>>.Success(paginatedResult);
    }
}