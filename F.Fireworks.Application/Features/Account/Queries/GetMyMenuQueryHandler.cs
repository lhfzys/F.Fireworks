using Ardalis.Result;
using F.Fireworks.Application.Contracts.Persistence;
using F.Fireworks.Application.Contracts.Services;
using F.Fireworks.Application.DTOs.Account;
using F.Fireworks.Shared.Enums;
using F.Fireworks.Shared.Utils;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace F.Fireworks.Application.Features.Account.Queries;

public class GetMyMenuQueryHandler(
    ICurrentUserService currentUser,
    IPermissionService permissionService,
    IApplicationDbContext context)
    : IRequestHandler<GetMyMenuQuery, Result<List<MenuNodeDto>>>
{
    public async Task<Result<List<MenuNodeDto>>> Handle(GetMyMenuQuery request, CancellationToken cancellationToken)
    {
        var userId = currentUser.UserId;
        if (userId is null) return Result<List<MenuNodeDto>>.Unauthorized();

        // 1. 获取用户拥有的所有权限Code
        var permissionCodes = await permissionService.GetPermissionCodesForUserAsync(userId.Value);
        if (permissionCodes.Count == 0) return Result<List<MenuNodeDto>>.Success([]);

        // 2. 从数据库中筛选出属于该用户的、且类型为菜单或目录的权限
        var menuPermissions = await context.Permissions
            .Where(p => permissionCodes.Contains(p.Code) &&
                        (p.Type == PermissionType.Menu || p.Type == PermissionType.Directory))
            .OrderBy(p => p.SortOrder)
            .Select(p => new MenuNodeDto
            {
                Id = p.Id,
                ParentId = p.ParentId,
                DisplayName = p.DisplayName,
                Path = p.Path,
                Icon = p.Icon,
                Code = p.Code,
                SortOrder = p.SortOrder
            })
            .ToListAsync(cancellationToken);
        var menuTree = TreeBuilder.BuildTree<MenuNodeDto, Guid>(menuPermissions);
        return Result<List<MenuNodeDto>>.Success(menuTree);
    }
}