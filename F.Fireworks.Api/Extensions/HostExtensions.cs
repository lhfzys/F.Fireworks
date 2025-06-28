using F.Fireworks.Domain.Identity;
using F.Fireworks.Infrastructure.Persistence;
using F.Fireworks.Infrastructure.Persistence.Seeders;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace F.Fireworks.Api.Extensions;

public static class HostExtensions
{
    public static async Task SeedDataAsync(this IHost host)
    {
        using var scope = host.Services.CreateScope();
        var services = scope.ServiceProvider;

        try
        {
            var context = services.GetRequiredService<ApplicationDbContext>();
            // 自动应用数据库迁移
            if ((await context.Database.GetPendingMigrationsAsync()).Any()) await context.Database.MigrateAsync();

            // 1. 首先植入权限
            var permissionSeeder = new PermissionSeeder(context);
            await permissionSeeder.SeedAsync(CancellationToken.None);

            // 2. 然后植入超级管理员
            var roleManager = services.GetRequiredService<RoleManager<ApplicationRole>>();
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
            var superAdminSeeder = new SuperAdminSeeder(roleManager, userManager, context);
            await superAdminSeeder.SeedAsync();
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred during data seeding.");
            throw;
        }
    }
}