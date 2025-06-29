using F.Fireworks.Domain.Identity;
using F.Fireworks.Domain.Logging;
using F.Fireworks.Domain.Permissions;
using F.Fireworks.Domain.Tenants;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace F.Fireworks.Application.Contracts.Persistence;

public interface IApplicationDbContext
{
    DbSet<Tenant> Tenants { get; }
    DbSet<Permission> Permissions { get; }
    DbSet<RefreshToken> RefreshTokens { get; }
    DbSet<AuditLog> AuditLogs { get; }
    DbSet<UserLoginLog> UserLoginLogs { get; }
    DbSet<ApplicationRolePermission> RolePermissions { get; }
    DbSet<IdentityUserRole<Guid>> UserRoles { get; }
    DbSet<ApplicationUser> Users { get; }
    DbSet<ApplicationRole> Roles { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}