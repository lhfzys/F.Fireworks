using Ardalis.Result;
using F.Fireworks.Application.Contracts.Persistence;
using F.Fireworks.Application.Contracts.Services;
using F.Fireworks.Application.DTOs.Permissions;
using F.Fireworks.Domain.Constants;
using F.Fireworks.Domain.Permissions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace F.Fireworks.Application.Features.Permissions.Queries;

public class GetAllPermissionsQueryHandler(IApplicationDbContext context, ICurrentUserService currentUser)
    : IRequestHandler<GetAllPermissionsQuery, Result<List<PermissionNodeDto>>>
{
    public async Task<Result<List<PermissionNodeDto>>> Handle(GetAllPermissionsQuery request,
        CancellationToken cancellationToken)
    {
        var query = context.Permissions.AsNoTracking();
        if (!currentUser.IsInRole(RoleConstants.SuperAdmin))
        {
            var tenantId = currentUser.TenantId;
            if (tenantId is null) return Result<List<PermissionNodeDto>>.Success([]);
            // 1. 找到当前租户所订阅的套餐ID
            var planId = await context.Tenants
                .Where(t => t.Id == tenantId.Value)
                .Select(t => t.PlanId)
                .FirstOrDefaultAsync(cancellationToken);
            if (planId is null) return Result<List<PermissionNodeDto>>.Success([]);
            // 2. 根据套餐ID，找出该套餐包含的所有权限ID
            var permissionIdsInPlan = await context.PlanPermissions
                .Where(pp => pp.PlanId == planId.Value)
                .Select(pp => pp.PermissionId)
                .ToListAsync(cancellationToken);
            // 3. 将主查询限定在这些权限ID内
            query = query.Where(p => permissionIdsInPlan.Contains(p.Id));
        }

        // 1. 从数据库一次性查询出所有权限，并按 SortOrder 排序
        var allPermissions = await query
            .OrderBy(p => p.SortOrder)
            .ToListAsync(cancellationToken);

        // 2. 将扁平列表转换为树状结构
        var tree = BuildPermissionTree(allPermissions);

        return Result<List<PermissionNodeDto>>.Success(tree);
    }

    private List<PermissionNodeDto> BuildPermissionTree(List<Permission> permissions)
    {
        // 使用字典进行高效查找，Key是权限ID，Value是对应的DTO
        var dictionary = new Dictionary<Guid, PermissionNodeDto>();

        // 最终返回的根节点列表
        var rootNodes = new List<PermissionNodeDto>();

        // 第一遍循环：创建所有节点的DTO实例，并放入字典
        foreach (var permission in permissions)
            dictionary[permission.Id] = new PermissionNodeDto
            {
                Id = permission.Id,
                Code = permission.Code,
                DisplayName = permission.DisplayName,
                Type = permission.Type
            };

        // 第二遍循环：构建层级关系
        foreach (var permission in permissions)
        {
            var node = dictionary[permission.Id];

            // 如果有父ID，并且在字典中能找到父节点DTO
            if (permission.ParentId.HasValue && dictionary.TryGetValue(permission.ParentId.Value, out var parentNode))
                // 将当前节点加到父节点的 Children 列表中
                parentNode.Children.Add(node);
            else
                // 如果没有父ID，说明是根节点
                rootNodes.Add(node);
        }

        return rootNodes;
    }
}