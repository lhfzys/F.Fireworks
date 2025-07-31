using F.Fireworks.Domain.Constants;
using F.Fireworks.Domain.Identity;
using F.Fireworks.Domain.Permissions;
using F.Fireworks.Domain.Tenants;
using F.Fireworks.Infrastructure.Options;
using F.Fireworks.Shared.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace F.Fireworks.Infrastructure.Persistence.Seeders;

public class SuperAdminSeeder(
    RoleManager<ApplicationRole> roleManager,
    UserManager<ApplicationUser> userManager,
    ApplicationDbContext context,
    IOptions<InitialSetupSettings> settingsOptions)
{
    private readonly InitialSetupSettings _settings = settingsOptions.Value;

    public async Task SeedAsync()
    {
        var adminRoleName = _settings.SuperAdminRoleName;
        var adminUserConfig = _settings.DefaultAdmin;
        const string systemTenantName = "System";

        if (adminRoleName != RoleConstants.SuperAdmin)
            throw new InvalidOperationException(
                $"Configuration mismatch: SuperAdminRoleName in appsettings ('{adminRoleName}') does not match the domain constant ('{RoleConstants.SuperAdmin}').");

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
        if (await userManager.FindByNameAsync(adminUserConfig.UserName) is null)
        {
            var adminUser = new ApplicationUser
            {
                UserName = adminUserConfig.UserName,
                Email = adminUserConfig.Email,
                EmailConfirmed = true,
                Status = UserStatus.Active,
                TenantId = systemTenant.Id
            };
            var result = await userManager.CreateAsync(adminUser, adminUserConfig.DefaultPassword);
            if (result.Succeeded) await userManager.AddToRoleAsync(adminUser, adminRoleName);
        }
    }
}