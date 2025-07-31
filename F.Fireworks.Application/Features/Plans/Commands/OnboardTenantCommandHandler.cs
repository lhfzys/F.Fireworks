using Ardalis.Result;
using F.Fireworks.Application.Contracts.Persistence;
using F.Fireworks.Domain.Identity;
using F.Fireworks.Domain.Permissions;
using F.Fireworks.Domain.Tenants;
using F.Fireworks.Shared.Enums;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace F.Fireworks.Application.Features.Plans.Commands;

public class OnboardTenantCommandHandler(
    IApplicationDbContext context,
    UserManager<ApplicationUser> userManager,
    RoleManager<ApplicationRole> roleManager) : IRequestHandler<OnboardTenantCommand, Result>
{
    public async Task<Result> Handle(OnboardTenantCommand request, CancellationToken cancellationToken)
    {
        // 预检查
        if (await context.Tenants.AnyAsync(t => t.Name == request.TenantName, cancellationToken))
            return Result.Invalid(new ValidationError("TenantName", $"租户 '{request.TenantName}' 已存在。"));
        if (await userManager.FindByNameAsync(request.AdminUserName) is not null)
            return Result.Invalid(new ValidationError("AdminUserName",
                $"用户名 '{request.AdminUserName}' 已存在。"));
        if (await userManager.FindByEmailAsync(request.AdminEmail) is not null)
            return Result.Invalid(new ValidationError("AdminEmail",
                $"邮箱 '{request.AdminEmail}' 已存在。"));

        // 开启数据库事务
        await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            // 1. 创建租户
            var tenant = new Tenant
            {
                Id = Guid.NewGuid(),
                Name = request.TenantName,
                Type = request.TenantType,
                PlanId = request.PlanId,
                IsActive = true
            };
            await context.Tenants.AddAsync(tenant, cancellationToken);

            // 2. 为新租户创建默认的“Admin”角色
            var adminRole = new ApplicationRole
                { Name = "Admin", Description = "Administrator role for the tenant", TenantId = tenant.Id };
            var roleResult = await roleManager.CreateAsync(adminRole);
            if (!roleResult.Succeeded)
            {
                await transaction.RollbackAsync(cancellationToken);
                var errorMessages = string.Join("\n", roleResult.Errors.Select(e => e.Description));
                return Result.Error($"无法创建租户管理员角色: {errorMessages}");
            }

            // 3. 为这个角色分配预设的租户管理权限
            var permissionsToAssign = await context.Permissions
                .Where(p => p.IsTenantPermission)
                .ToListAsync(cancellationToken);
            var rolePermissions = permissionsToAssign.Select(p => new ApplicationRolePermission
                { RoleId = adminRole.Id, PermissionId = p.Id });
            await context.RolePermissions.AddRangeAsync(rolePermissions, cancellationToken);

            // 4. 创建租户的第一个管理员用户
            var adminUser = new ApplicationUser
            {
                Email = request.AdminEmail,
                UserName = request.AdminUserName,
                TenantId = tenant.Id,
                EmailConfirmed = true,
                MustChangePassword = true,
                Status = UserStatus.Active
            };
            var userResult = await userManager.CreateAsync(adminUser, request.AdminPassword);
            if (!userResult.Succeeded)
            {
                await transaction.RollbackAsync(cancellationToken);
                var errorMessages = string.Join("\n", userResult.Errors.Select(e => e.Description));
                return Result.Error($"无法创建租户管理员账户: {errorMessages}");
            }

            // 5. 将“Admin”角色分配给这个用户
            context.UserRoles.Add(new IdentityUserRole<Guid>
            {
                UserId = adminUser.Id,
                RoleId = adminRole.Id
            });


            // 6. 一次性保存所有更改
            await context.SaveChangesAsync(cancellationToken);

            // 7. 提交事务
            await transaction.CommitAsync(cancellationToken);

            return Result.Success();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            return Result.Error("引导新租户时发生意外错误。");
        }
    }
}