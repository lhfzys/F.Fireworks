using F.Fireworks.Application.Contracts.Persistence;
using F.Fireworks.Application.Contracts.Services;
using F.Fireworks.Domain.Common;
using F.Fireworks.Domain.Courses;
using F.Fireworks.Domain.Identity;
using F.Fireworks.Domain.Logging;
using F.Fireworks.Domain.Permissions;
using F.Fireworks.Domain.Subscriptions;
using F.Fireworks.Domain.Tenants;
using F.Fireworks.Infrastructure.Persistence.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace F.Fireworks.Infrastructure.Persistence;

public class ApplicationDbContext(
    DbContextOptions<ApplicationDbContext> options,
    ICurrentUserService currentUserService)
    : IdentityDbContext<ApplicationUser, ApplicationRole, Guid,
        IdentityUserClaim<Guid>, IdentityUserRole<Guid>, IdentityUserLogin<Guid>,
        IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>(options), IApplicationDbContext
{
    // --- 业务相关的实体 ---
    public DbSet<Plan> Plans => Set<Plan>();
    public DbSet<PlanPermission> PlanPermissions => Set<PlanPermission>();
    public DbSet<Tenant> Tenants => Set<Tenant>();
    public DbSet<Permission> Permissions => Set<Permission>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    // --- 日志相关的实体 ---
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();
    public DbSet<UserLoginLog> UserLoginLogs => Set<UserLoginLog>();

    // --- 连接实体 ---
    // 这个实体用于配置多对多关系，EF Core 会自动处理
    public DbSet<ApplicationRolePermission> RolePermissions => Set<ApplicationRolePermission>();

    public DbSet<Grade> Grades => Set<Grade>();
    public DbSet<LibraryLesson> LibraryLessons => Set<LibraryLesson>();
    public DbSet<LibraryTopic> LibraryTopics => Set<LibraryTopic>();

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        HandleAuditing();
        HandleSoftDelete();

        // 调用基类的原始方法，以执行实际的保存操作
        return await base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyIdentityTableNamingConvention();
        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }

    private void HandleAuditing()
    {
        var userId = currentUserService.UserId;
        var utcNow = DateTime.UtcNow;

        // 只查找实现了 IAuditable 接口的实体
        foreach (var entry in ChangeTracker.Entries<IAuditable>())
            switch (entry.State)
            {
                // 如果实体是新添加的
                case EntityState.Added:
                    entry.Entity.CreatedBy = userId;
                    entry.Entity.CreatedOn = utcNow;
                    break;
                // 如果实体是被修改的
                case EntityState.Modified:
                    entry.Entity.LastModifiedBy = userId;
                    entry.Entity.LastModifiedOn = utcNow;
                    break;
            }
    }

    private void HandleSoftDelete()
    {
        var userId = currentUserService.UserId;
        var utcNow = DateTime.UtcNow;

        // 只查找实现了 ISoftDeletable 接口且状态为“已删除”的实体
        foreach (var entry in ChangeTracker.Entries<ISoftDeletable>().Where(e => e.State == EntityState.Deleted))
        {
            // 1. 将状态从“已删除”强制修改为“已修改”
            //    这样 EF Core 就不会真的从数据库中删除这条记录
            entry.State = EntityState.Modified;

            // 2. 填充软删除相关的字段
            entry.Entity.DeletedBy = userId;
            entry.Entity.DeletedOn = utcNow;
            entry.Entity.IsDeleted = true;
        }
    }
}