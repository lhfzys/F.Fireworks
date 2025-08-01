using Ardalis.Result;
using F.Fireworks.Application.Contracts.Persistence;
using F.Fireworks.Application.Contracts.Services;
using F.Fireworks.Application.DTOs.Account;
using F.Fireworks.Domain.Identity;
using F.Fireworks.Shared.Enums;
using F.Fireworks.Shared.Utils;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace F.Fireworks.Application.Features.Account.Queries;

public class GetMyProfileQueryHandler(
    ICurrentUserService currentUser,
    UserManager<ApplicationUser> userManager,
    IApplicationDbContext context,
    IPermissionService permissionService)
    : IRequestHandler<GetMyProfileQuery, Result<UserProfileDto>>
{
    private readonly IApplicationDbContext _context = context;

    public async Task<Result<UserProfileDto>> Handle(GetMyProfileQuery request, CancellationToken cancellationToken)
    {
        var userId = currentUser.UserId;
        if (userId is null) return Result<UserProfileDto>.Unauthorized();
        // 1. 获取用户基本信息和关联的租户信息
        var user = await userManager.Users
            .AsNoTracking()
            .Include(u => u.Tenant)
            .FirstOrDefaultAsync(u => u.Id == userId.Value, cancellationToken);
        if (user is null) return Result<UserProfileDto>.NotFound("User not found.");

        // 2. 获取用户的角色列表
        var roles = await userManager.GetRolesAsync(user);

        // 3. 获取用户的所有权限Code (调用新创建的服务)
        var permissions = await permissionService.GetPermissionCodesForUserAsync(user.Id);

        var menuPermissions = await context.Permissions
            .Where(p => permissions.Contains(p.Code) &&
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
        var profileDto = new UserProfileDto
        {
            Id = user.Id,
            UserName = user.UserName,
            Status = user.Status,

            Tenant = new TenantInfo(user.Tenant.Id, user.Tenant.Name),
            Roles = roles.ToList(),
            Permissions = permissions,
            Menus = menuTree
        };

        return Result<UserProfileDto>.Success(profileDto);
    }
}