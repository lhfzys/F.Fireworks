using F.Fireworks.Domain.Identity;
using F.Fireworks.Domain.Permissions;
using F.Fireworks.Domain.Tenants;
using F.Fireworks.Shared.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace F.Fireworks.Infrastructure.Persistence.Seeders;

public class SuperAdminSeeder(
    RoleManager<ApplicationRole> roleManager,
    UserManager<ApplicationUser> userManager,
    ApplicationDbContext context)
{
    public async Task SeedAsync()
    {
        const string adminRoleName = "SuperAdmin";
        const string adminUserName = "superadmin";
        const string systemTenantName = "System";

        var systemTenant = await context.Tenants.FirstOrDefaultAsync(t => t.Name == systemTenantName);
        if (systemTenant is null)
        {
            systemTenant = new Tenant
            {
                Id = Guid.NewGuid(),
                Name = systemTenantName,
                Type = TenantType.System,
                IsActive = true
            };
            await context.Tenants.AddAsync(systemTenant);
            await context.SaveChangesAsync();
        }

        // 1. 植入 SuperAdmin 角色
        if (await roleManager.FindByNameAsync(adminRoleName) is not { } adminRole)
        {
            adminRole = new ApplicationRole
            {
                Name = adminRoleName, Description = "Super administrator with all permissions.",
                TenantId = Guid.Empty
            };
            await roleManager.CreateAsync(adminRole);
        }

        // 2. 为 SuperAdmin 角色授予所有权限
        var allPermissions = await context.Permissions.ToListAsync();
        var rolePermissions = await context.RolePermissions
            .Where(rp => rp.RoleId == adminRole.Id)
            .Select(rp => rp.PermissionId)
            .ToListAsync();

        var newPermissions = allPermissions
            .Where(p => !rolePermissions.Contains(p.Id))
            .Select(p => new ApplicationRolePermission { RoleId = adminRole.Id, PermissionId = p.Id })
            .ToList();

        if (newPermissions.Any())
        {
            await context.RolePermissions.AddRangeAsync(newPermissions);
            await context.SaveChangesAsync();
        }

        // 3. 植入 SuperAdmin 用户
        if (await userManager.FindByNameAsync(adminUserName) is null)
        {
            var adminUser = new ApplicationUser
            {
                UserName = adminUserName,
                Email = "superadmin@yourproject.com",
                EmailConfirmed = true,
                Status = UserStatus.Active,
                TenantId = systemTenant.Id
            };
            var result = await userManager.CreateAsync(adminUser, "123456");
            if (result.Succeeded) await userManager.AddToRoleAsync(adminUser, adminRoleName);
        }
    }
}