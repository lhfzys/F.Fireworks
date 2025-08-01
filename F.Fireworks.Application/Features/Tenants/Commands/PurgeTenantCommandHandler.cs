using Ardalis.Result;
using F.Fireworks.Application.Contracts.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace F.Fireworks.Application.Features.Tenants.Commands;

public class PurgeTenantCommandHandler(IApplicationDbContext context) : IRequestHandler<PurgeTenantCommand, Result>
{
    public async Task<Result> Handle(PurgeTenantCommand request, CancellationToken cancellationToken)
    {
        var tenantId = request.TenantId;
        if (!await context.Tenants.AnyAsync(t => t.Id == tenantId, cancellationToken))
            return Result.NotFound("租户不存在或已被删除");

        await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            // 1. 收集所有需要删除的关联实体的ID
            var userIds = await context.Users.Where(u => u.TenantId == tenantId).Select(u => u.Id)
                .ToListAsync(cancellationToken);
            var roleIds = await context.Roles.Where(r => r.TenantId == tenantId).Select(r => r.Id)
                .ToListAsync(cancellationToken);

            // 删除最末端的依赖：日志、令牌等
            if (userIds.Count != 0)
            {
                await context.RefreshTokens.Where(rt => userIds.Contains(rt.UserId!))
                    .ExecuteDeleteAsync(cancellationToken);
                await context.UserLoginLogs.Where(ull => userIds.Contains(ull.UserId!.Value))
                    .ExecuteDeleteAsync(cancellationToken);
            }

            // 删除用户和角色的关联
            if (userIds.Count != 0)
                await context.UserRoles.Where(ur => userIds.Contains(ur.UserId)).ExecuteDeleteAsync(cancellationToken);

            // 删除角色和权限的关联
            if (roleIds.Count != 0)
                await context.RolePermissions.Where(rp => roleIds.Contains(rp.RoleId))
                    .ExecuteDeleteAsync(cancellationToken);

            if (roleIds.Count != 0)
                await context.Roles.Where(r => roleIds.Contains(r.Id)).ExecuteDeleteAsync(cancellationToken);

            if (userIds.Count != 0)
                await context.Users.Where(u => userIds.Contains(u.Id)).ExecuteDeleteAsync(cancellationToken);

            // 3. 最后，删除租户本身
            await context.Tenants.Where(t => t.Id == tenantId).ExecuteDeleteAsync(cancellationToken);

            // 4. 提交事务
            await transaction.CommitAsync(cancellationToken);

            return Result.SuccessWithMessage("租户相关数据已被清空");
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            return Result.Error($"清空数据时出错: {ex.Message}");
        }
    }
}