using F.Fireworks.Application.Contracts.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace F.Fireworks.Infrastructure.Auth;

public class PermissionAuthorizationHandler(IApplicationDbContext context) : AuthorizationHandler<PermissionRequirement>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context1,
        PermissionRequirement requirement)
    {
        var userIdString = context1.User.Claims.FirstOrDefault(c => c.Type == "uid")?.Value;
        if (!Guid.TryParse(userIdString, out var userId)) return;
        var requiredPermissionCode = requirement.Permission;
        var userRoleIds = await context.UserRoles
            .Where(ur => ur.UserId == userId)
            .Select(ur => ur.RoleId)
            .ToListAsync();
        if (!userRoleIds.Any()) return;
        var hasPermission = await context.RolePermissions
            .Where(rp => userRoleIds.Contains(rp.RoleId)) // 过滤出用户拥有的角色的权限分配记录
            .Join(context.Permissions, // 与权限表连接
                rp => rp.PermissionId,
                p => p.Id,
                (rp, p) => p.Code) // 只选择出权限的Code
            .AnyAsync(code => code == requiredPermissionCode);
        if (hasPermission)
            context1.Succeed(requirement);
    }
}