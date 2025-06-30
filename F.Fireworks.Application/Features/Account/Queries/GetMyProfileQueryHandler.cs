using Ardalis.Result;
using F.Fireworks.Application.Contracts.Persistence;
using F.Fireworks.Application.Contracts.Services;
using F.Fireworks.Application.DTOs.Account;
using F.Fireworks.Domain.Identity;
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


        var profileDto = new UserProfileDto
        {
            Id = user.Id,
            UserName = user.UserName,
            Status = user.Status,

            Tenant = new TenantInfo(user.Tenant.Id, user.Tenant.Name),
            Roles = roles.ToList(),
            Permissions = permissions
        };

        return Result<UserProfileDto>.Success(profileDto);
    }
}