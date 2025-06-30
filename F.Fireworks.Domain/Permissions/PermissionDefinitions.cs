using F.Fireworks.Shared.Enums;

namespace F.Fireworks.Domain.Permissions;

public static class PermissionDefinitions
{
    // 注意 FieldName 定义后不要改变
    // 系统管理目录
    [Permission("系统管理", PermissionType.Directory, 100, "/system")]
    public const string System = "System";

    // 用户管理
    [Permission("用户管理", PermissionType.Menu, 1, "/system/users", System)]
    public const string Users = "System.Users";

    [Permission("查看用户", PermissionType.Button, 1, parentCode: Users)]
    public const string UsersRead = "System.Users.Read";

    [Permission("创建用户", PermissionType.Button, 2, parentCode: Users)]
    public const string UsersCreate = "System.Users.Create";

    [Permission("编辑用户", PermissionType.Button, 3, parentCode: Users)]
    public const string UsersUpdate = "System.Users.Update";

    [Permission("删除用户", PermissionType.Button, 4, parentCode: Users)]
    public const string UsersDelete = "System.Users.Delete";

    [Permission("分配角色", PermissionType.Button, 5, parentCode: Users)]
    public const string UsersUpdateRoles = "System.Users.UpdateRoles";

    // 角色管理
    [Permission("角色管理", PermissionType.Menu, 2, "/system/roles", System)]
    public const string Roles = "System.Roles";

    [Permission("查看角色", PermissionType.Button, 1, parentCode: Roles)]
    public const string RolesRead = "System.Roles.Read";

    [Permission("创建角色", PermissionType.Button, 2, parentCode: Roles)]
    public const string RolesCreate = "System.Roles.Create";

    [Permission("编辑角色", PermissionType.Button, 3, parentCode: Roles)]
    public const string RolesUpdate = "System.Roles.Update";

    [Permission("删除角色", PermissionType.Button, 4, parentCode: Roles)]
    public const string RolesDelete = "System.Roles.Delete";

    // 权限管理
    [Permission("权限管理", PermissionType.Menu, 1, "/system/permissions", System)]
    public const string Permissions = "System.Permissions";

    [Permission("查看权限", PermissionType.Button, 1, parentCode: Permissions)]
    public const string PermissionsRead = "System.Permissions.Read";
}