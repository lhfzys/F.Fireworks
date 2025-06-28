using Ardalis.Result;
using F.Fireworks.Application.Contracts.Persistence;
using F.Fireworks.Application.DTOs.Permissions;
using F.Fireworks.Domain.Permissions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace F.Fireworks.Application.Features.Permissions.Queries;

public class GetAllPermissionsQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetAllPermissionsQuery, Result<List<PermissionNodeDto>>>
{
    public async Task<Result<List<PermissionNodeDto>>> Handle(GetAllPermissionsQuery request,
        CancellationToken cancellationToken)
    {
        // 1. 从数据库一次性查询出所有权限，并按 SortOrder 排序
        var allPermissions = await context.Permissions
            .AsNoTracking() // 只读查询，提升性能
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