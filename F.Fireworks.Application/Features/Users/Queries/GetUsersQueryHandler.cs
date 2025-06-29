using Ardalis.Result;
using F.Fireworks.Application.Common.Extensions;
using F.Fireworks.Application.DTOs.Common;
using F.Fireworks.Application.DTOs.Users;
using F.Fireworks.Domain.Identity;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace F.Fireworks.Application.Features.Users.Queries;

public class GetUsersQueryHandler(UserManager<ApplicationUser> userManager)
    : IRequestHandler<GetUsersQuery, Result<PaginatedList<UserDto>>>
{
    public async Task<Result<PaginatedList<UserDto>>> Handle(GetUsersQuery request,
        CancellationToken cancellationToken)
    {
        var paginatedResult = await userManager.Users
            .AsNoTracking()
            .ApplyFiltering(request.Filter) // 步骤 1: 自动应用所有筛选
            .ApplySort(request.Filter.SortField, request.Filter.SortOrder) // 步骤 2: 应用排序
            .ProjectToType<UserDto>() // 步骤 3: 高性能投影
            .ToPaginatedListAsync(request.Filter.PageNumber, request.Filter.PageSize, cancellationToken); // 步骤 4: 分页
        return Result<PaginatedList<UserDto>>.Success(paginatedResult);
    }
}