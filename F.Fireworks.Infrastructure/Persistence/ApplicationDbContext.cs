using F.Fireworks.Application.Contracts.Persistence;
using F.Fireworks.Domain.Identity;
using F.Fireworks.Domain.Logging;
using F.Fireworks.Domain.Permissions;
using F.Fireworks.Domain.Tenants;
using F.Fireworks.Infrastructure.Persistence.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace F.Fireworks.Infrastructure.Persistence;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<ApplicationUser, ApplicationRole, Guid,
        IdentityUserClaim<Guid>, IdentityUserRole<Guid>, IdentityUserLogin<Guid>,
        IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>(options), IApplicationDbContext
{
    // --- 业务相关的实体 ---
    public DbSet<Tenant> Tenants => Set<Tenant>();
    public DbSet<Permission> Permissions => Set<Permission>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    // --- 日志相关的实体 ---
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();
    public DbSet<UserLoginLog> UserLoginLogs => Set<UserLoginLog>();

    // --- 连接实体 ---
    // 这个实体用于配置多对多关系，EF Core 会自动处理
    public DbSet<ApplicationRolePermission> RolePermissions => Set<ApplicationRolePermission>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyIdentityTableNamingConvention();
        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}