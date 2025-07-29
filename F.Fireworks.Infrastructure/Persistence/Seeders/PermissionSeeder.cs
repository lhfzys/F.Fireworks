using System.Reflection;
using EFCore.BulkExtensions;
using F.Fireworks.Domain.Permissions;
using F.Fireworks.Infrastructure.Common;
using Microsoft.EntityFrameworkCore;

namespace F.Fireworks.Infrastructure.Persistence.Seeders;

public class PermissionSeeder(ApplicationDbContext context)
{
    public async Task SeedAsync(CancellationToken cancellationToken)
    {
        var codePermissions = GetPermissionsFromCode();
        var dbPermissions = await context.Permissions.ToListAsync(cancellationToken);

        var newPermissions = codePermissions.Where(p => dbPermissions.All(dp => dp.Code != p.Code)).ToList();
        var updatedPermissions = new List<Permission>();

        foreach (var dbPermission in dbPermissions)
        {
            var codePermission = codePermissions.FirstOrDefault(p => p.Code == dbPermission.Code);
            if (codePermission != null)
            {
                // 比较关键字段是否有变化
                var needsUpdate = dbPermission.DisplayName != codePermission.DisplayName ||
                                  dbPermission.ParentId != codePermission.ParentId ||
                                  dbPermission.SortOrder != codePermission.SortOrder ||
                                  dbPermission.Path != codePermission.Path ||
                                  dbPermission.Type != codePermission.Type || dbPermission.IsTenantPermission !=
                                  codePermission.IsTenantPermission;
                if (needsUpdate)
                {
                    dbPermission.DisplayName = codePermission.DisplayName;
                    dbPermission.ParentId = codePermission.ParentId;
                    dbPermission.SortOrder = codePermission.SortOrder;
                    dbPermission.Path = codePermission.Path;
                    dbPermission.Type = codePermission.Type;
                    dbPermission.IsTenantPermission = codePermission.IsTenantPermission;
                    updatedPermissions.Add(dbPermission);
                }
            }
        }

        if (newPermissions.Count != 0 || updatedPermissions.Count != 0)
        {
            await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);

            if (newPermissions.Count != 0)
                // 使用 BulkExtensions 批量插入，高效处理“不可能一条一条插入”的问题
                await context.BulkInsertAsync(newPermissions, cancellationToken: cancellationToken);

            if (updatedPermissions.Count != 0)
                // 使用 BulkExtensions 批量更新
                await context.BulkUpdateAsync(updatedPermissions, cancellationToken: cancellationToken);

            await transaction.CommitAsync(cancellationToken);
        }
    }

    private List<Permission> GetPermissionsFromCode()
    {
        var permissions = new List<Permission>();
        var permissionDefinitionsType = typeof(PermissionDefinitions);
        var fields =
            permissionDefinitionsType.GetFields(BindingFlags.Public | BindingFlags.Static |
                                                BindingFlags.FlattenHierarchy);
        var permissionWithParentCode = new List<(Permission permission, string? parentCode)>();
        foreach (var field in fields)
        {
            var code = field.GetValue(null)?.ToString();
            var attribute = field.GetCustomAttribute<PermissionAttribute>();
            var fieldName = field.Name;

            if (code != null && attribute != null)
            {
                var permission = new Permission
                {
                    Id = DeterministicGuidGenerator.Create(fieldName),
                    Code = code,
                    DisplayName = attribute.Description,
                    Type = attribute.Type,
                    SortOrder = attribute.SortOrder,
                    Path = attribute.Path,
                    IsTenantPermission = attribute.IsTenantPermission
                };
                permissions.Add(permission);
                permissionWithParentCode.Add((permission, attribute.ParentCode));
            }
        }

        foreach (var item in permissionWithParentCode)
            if (!string.IsNullOrEmpty(item.parentCode))
            {
                var parent = permissions.FirstOrDefault(p => p.Code == item.parentCode);
                if (parent != null) item.permission.ParentId = parent.Id;
            }

        return permissions;
    }
}