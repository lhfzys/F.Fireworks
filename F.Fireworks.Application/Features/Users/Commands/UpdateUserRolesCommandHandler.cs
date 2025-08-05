using Ardalis.Result;
using F.Fireworks.Application.Contracts.Persistence;
using F.Fireworks.Application.Contracts.Services;
using F.Fireworks.Domain.Constants;
using F.Fireworks.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace F.Fireworks.Application.Features.Users.Commands;

public class UpdateUserRolesCommandHandler(
    UserManager<ApplicationUser> userManager,
    ICurrentUserService currentUser,
    IApplicationDbContext context)
    : IRequestHandler<UpdateUserRolesCommand, Result>
{
    public async Task<Result> Handle(UpdateUserRolesCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(request.UserId.ToString());
        if (user is null) return Result.NotFound("用户不存在或已被删除");
        if (!currentUser.IsInRole("SuperAdmin") && user.TenantId != currentUser.TenantId)
            return Result.Forbidden("用户不存在或已被删除");

        var targetTenantId =
            currentUser.IsInRole(RoleConstants.SuperAdmin) ? user.TenantId : currentUser.TenantId!.Value;

        var validRolesInTenantCount = await context.Roles
            .CountAsync(r => r.TenantId == targetTenantId && request.RoleIds.Contains(r.Id), cancellationToken);
        if (validRolesInTenantCount != request.RoleIds.Count)
            return Result.Invalid(new ValidationError("RoleIds", "一个或多个角色不存在，或不属于目标租户。"));


        // 1. 找出用户当前拥有的角色
        var currentUserRoles =
            await context.UserRoles.Where(ur => ur.UserId == request.UserId).ToListAsync(cancellationToken);
        var currentRoleIds = currentUserRoles.Select(ur => ur.RoleId);

        // 2. 计算需要移除的角色
        var rolesToRemove = currentUserRoles.Where(ur => !request.RoleIds.Contains(ur.RoleId)).ToList();

        // 3. 计算需要新增的角色ID
        var roleIdsToAdd = request.RoleIds.Except(currentRoleIds).ToList();

        // 4. 执行数据库操作
        if (rolesToRemove.Any()) context.UserRoles.RemoveRange(rolesToRemove);

        if (roleIdsToAdd.Any())
        {
            var newUserRoles = roleIdsToAdd.Select(roleId => new IdentityUserRole<Guid>
            {
                UserId = request.UserId,
                RoleId = roleId
            });
            await context.UserRoles.AddRangeAsync(newUserRoles, cancellationToken);
        }

        await context.SaveChangesAsync(cancellationToken);

        return Result.SuccessWithMessage("用户角色已成功更新。");
    }
}