using F.Fireworks.Application.Contracts.Persistence;
using F.Fireworks.Application.Contracts.Services;
using Microsoft.EntityFrameworkCore;

namespace F.Fireworks.Infrastructure.Services;

public class PermissionService(IApplicationDbContext context) : IPermissionService
{
    public async Task<List<string>> GetPermissionCodesForUserAsync(Guid userId)
    {
        var userRoleIds = await context.UserRoles
            .Where(ur => ur.UserId == userId)
            .Select(ur => ur.RoleId)
            .ToListAsync();

        if (userRoleIds.Count == 0) return [];

        return await context.RolePermissions
            .Where(rp => userRoleIds.Contains(rp.RoleId))
            .Include(rp => rp.Permission)
            .Select(rp => rp.Permission.Code)
            .Distinct()
            .ToListAsync();
    }
}