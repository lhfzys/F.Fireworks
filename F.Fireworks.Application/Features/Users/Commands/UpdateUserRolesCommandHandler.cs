using Ardalis.Result;
using F.Fireworks.Application.Contracts.Persistence;
using F.Fireworks.Application.Contracts.Services;
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
        if (!currentUser.IsInRole("SuperAdmin"))
        {
            var tenantId = currentUser.TenantId;
            var rolesInTenantCount = await context.Roles
                .CountAsync(r => r.TenantId == tenantId && request.RoleNames.Contains(r.Name), cancellationToken);

            if (rolesInTenantCount != request.RoleNames.Count)
                return Result.Invalid(new ValidationError
                {
                    Identifier = "RoleNames",
                    ErrorMessage = "有角色不属于该租户"
                });
        }

        // 获取用户当前的角色
        var currentRoles = await userManager.GetRolesAsync(user);

        // 计算需要添加的角色 (新角色列表有，但当前角色列表没有)
        var rolesToAdd = request.RoleNames.Except(currentRoles).ToList();
        if (rolesToAdd.Count != 0)
        {
            var addResult = await userManager.AddToRolesAsync(user, rolesToAdd);
            if (!addResult.Succeeded)
            {
                var errors = addResult.Errors
                    .Select(e => new ValidationError { Identifier = e.Code, ErrorMessage = e.Description })
                    .ToList();
                return Result.Invalid(errors);
            }
        }

        var rolesToRemove = currentRoles.Except(request.RoleNames).ToList();
        if (rolesToRemove.Any())
        {
            var removeResult = await userManager.RemoveFromRolesAsync(user, rolesToRemove);
            if (!removeResult.Succeeded)
            {
                var errors = removeResult.Errors
                    .Select(e => new ValidationError { Identifier = e.Code, ErrorMessage = e.Description })
                    .ToList();
                return Result.Invalid(errors);
            }
        }

        return Result.Success();
    }
}