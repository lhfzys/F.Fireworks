using F.Fireworks.Application.Contracts.Persistence;
using F.Fireworks.Application.Contracts.Services;
using F.Fireworks.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace F.Fireworks.Infrastructure.Auth;

public class PermissionAuthorizationHandler(IApplicationDbContext context, ICurrentUserService currentUser)
    : AuthorizationHandler<PermissionRequirement>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context1,
        PermissionRequirement requirement)
    {
        // 1. 超级管理员“快速通道”
        if (currentUser.IsInRole(RoleConstants.SuperAdmin))
        {
            context1.Succeed(requirement);
            return;
        }

        // 2. 归属检查：非超管用户必须有租户ID
        var userId = currentUser.UserId;
        var tenantId = currentUser.TenantId;
        if (userId is null || tenantId is null) return;

        // 3. 套餐检查：检查租户的套餐是否包含该权限
        // 首先获取租户的套餐ID
        var planId = await context.Tenants
            .Where(t => t.Id == tenantId.Value && t.IsActive && !t.IsDeleted)
            .Select(t => t.PlanId)
            .FirstOrDefaultAsync();

        if (planId is null)
            // 如果租户没有订阅任何有效套餐，则没有任何权限
            return; // 验证失败

        // 检查该套餐是否包含所需权限
        var isPermissionInPlan = await context.PlanPermissions
            .AnyAsync(pp => pp.PlanId == planId.Value && pp.Permission.Code == requirement.Permission);
        if (!isPermissionInPlan) return; // 验证失败
        // 4. ✅ 分配检查：在确认租户有权使用此功能后，再检查用户是否被分配了此权限
        var requiredPermissionCode = requirement.Permission;
        var hasPermissionAssigned = await context.UserRoles
            .Where(ur => ur.UserId == userId.Value)
            .Join(context.RolePermissions,
                userRole => userRole.RoleId,
                rolePermission => rolePermission.RoleId,
                (userRole, rolePermission) => rolePermission.PermissionId)
            .Join(context.Permissions,
                permissionId => permissionId,
                permission => permission.Id,
                (permissionId, permission) => permission.Code)
            .AnyAsync(code => code == requiredPermissionCode);

        if (hasPermissionAssigned) context1.Succeed(requirement);
    }
}