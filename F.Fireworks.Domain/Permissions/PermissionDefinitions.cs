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
    [Permission("权限管理", PermissionType.Menu, 3, "/system/permissions", System)]
    public const string Permissions = "System.Permissions";

    [Permission("查看权限", PermissionType.Button, 1, parentCode: Permissions)]
    public const string PermissionsRead = "System.Permissions.Read";

    // 租户管理
    [Permission("租户管理", PermissionType.Menu, 4, "/system/tenants", System)]
    public const string Tenants = "System.Tenants";

    [Permission("查看租户", PermissionType.Button, 1, parentCode: Tenants)]
    public const string TenantsRead = "System.Tenants.Read";

    [Permission("创建租户", PermissionType.Button, 2, parentCode: Tenants)]
    public const string TenantsCreate = "System.Tenants.Create";

    [Permission("编辑租户", PermissionType.Button, 3, parentCode: Tenants)]
    public const string TenantsUpdate = "System.Tenants.Update";

    [Permission("删除租户", PermissionType.Button, 4, parentCode: Tenants)]
    public const string TenantsDelete = "System.Tenants.Delete";

    [Permission("会话管理", PermissionType.Menu, 5, "/system/sessions", System)]
    public const string Sessions = "System.Sessions";

    [Permission("查看会话", PermissionType.Button, 1, parentCode: Sessions)]
    public const string SessionsRead = "System.Sessions.Read";

    [Permission("强制下线", PermissionType.Button, 2, parentCode: Sessions)]
    public const string SessionsRevoke = "System.Sessions.Revoke";

    [Permission("内容库管理", PermissionType.Directory, 200, "/library")]
    public const string Library = "Library";

    [Permission("年级管理", PermissionType.Menu, 1, "/library/grades", Library)]
    public const string LibraryGrades = "Library.Grades";

    [Permission("查看年级", PermissionType.Button, 1, parentCode: LibraryGrades)]
    public const string LibraryGradesRead = "Library.Grades.Read";

    [Permission("创建年级", PermissionType.Button, 2, parentCode: LibraryGrades)]
    public const string LibraryGradesCreate = "Library.Grades.Create";

    [Permission("编辑年级", PermissionType.Button, 3, parentCode: LibraryGrades)]
    public const string LibraryGradesUpdate = "Library.Grades.Update";

    [Permission("删除年级", PermissionType.Button, 4, parentCode: LibraryGrades)]
    public const string LibraryGradesDelete = "Library.Grades.Delete";

    [Permission("专题管理", PermissionType.Menu, 2, "/library/topics", Library)]
    public const string LibraryTopics = "Library.Topics";

    [Permission("查看专题", PermissionType.Button, 1, parentCode: LibraryTopics)]
    public const string LibraryTopicsRead = "Library.Topics.Read";

    [Permission("创建专题", PermissionType.Button, 2, parentCode: LibraryTopics)]
    public const string LibraryTopicsCreate = "Library.Topics.Create";

    [Permission("编辑专题", PermissionType.Button, 3, parentCode: LibraryTopics)]
    public const string LibraryTopicsUpdate = "Library.Topics.Update";

    [Permission("删除专题", PermissionType.Button, 4, parentCode: LibraryTopics)]
    public const string LibraryTopicsDelete = "Library.Topics.Delete";

    public const string LibraryLessons = "Library.Lessons";

    [Permission("查看课节", PermissionType.Button, 1, parentCode: LibraryTopics)]
    public const string LibraryLessonsRead = "Library.Lessons.Read";

    [Permission("创建课节", PermissionType.Button, 2, parentCode: LibraryTopics)]
    public const string LibraryLessonsCreate = "Library.Lessons.Create";

    [Permission("编辑课节", PermissionType.Button, 3, parentCode: LibraryTopics)]
    public const string LibraryLessonsUpdate = "Library.Lessons.Update";

    [Permission("删除课节", PermissionType.Button, 4, parentCode: LibraryTopics)]
    public const string LibraryLessonsDelete = "Library.Lessons.Delete";
}