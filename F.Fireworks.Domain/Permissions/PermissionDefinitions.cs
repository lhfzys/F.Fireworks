using F.Fireworks.Shared.Enums;

namespace F.Fireworks.Domain.Permissions;

public static class PermissionDefinitions
{
    // 注意 FieldName 定义后不要改变
    [Permission("Dashboard", PermissionType.Directory, 0, true, "/dashboard")]
    public const string Dashboard = "Dashboard";

    [Permission("首页", PermissionType.Menu, 1, true, "/dashboard", Dashboard)]
    public const string Home = "Dashboard.Home";

    // 系统管理目录
    [Permission("系统管理", PermissionType.Directory, 100, true, "/system")]
    public const string System = "System";

    // 用户管理
    [Permission("用户管理", PermissionType.Menu, 1, true, "/system/users", System)]
    public const string Users = "System.Users";

    [Permission("查看用户", PermissionType.Button, 1, true, parentCode: Users)]
    public const string UsersRead = "System.Users.Read";

    [Permission("创建用户", PermissionType.Button, 2, true, parentCode: Users)]
    public const string UsersCreate = "System.Users.Create";

    [Permission("编辑用户", PermissionType.Button, 3, true, parentCode: Users)]
    public const string UsersUpdate = "System.Users.Update";

    [Permission("删除用户", PermissionType.Button, 4, true, parentCode: Users)]
    public const string UsersDelete = "System.Users.Delete";

    [Permission("分配角色", PermissionType.Button, 5, true, parentCode: Users)]
    public const string UsersUpdateRoles = "System.Users.UpdateRoles";

    // 角色管理
    [Permission("角色管理", PermissionType.Menu, 2, true, "/system/roles", System)]
    public const string Roles = "System.Roles";

    [Permission("查看角色", PermissionType.Button, 1, true, parentCode: Roles)]
    public const string RolesRead = "System.Roles.Read";

    [Permission("创建角色", PermissionType.Button, 2, true, parentCode: Roles)]
    public const string RolesCreate = "System.Roles.Create";

    [Permission("编辑角色", PermissionType.Button, 3, true, parentCode: Roles)]
    public const string RolesUpdate = "System.Roles.Update";

    [Permission("删除角色", PermissionType.Button, 4, true, parentCode: Roles)]
    public const string RolesDelete = "System.Roles.Delete";

    // 权限管理
    [Permission("权限管理", PermissionType.Menu, 3, false, "/system/permissions", System)]
    public const string Permissions = "System.Permissions";

    [Permission("查看权限", PermissionType.Button, 1, true, parentCode: Permissions)]
    public const string PermissionsRead = "System.Permissions.Read";

    // 租户管理
    [Permission("租户管理", PermissionType.Menu, 4, false, "/system/tenants", System)]
    public const string Tenants = "System.Tenants";

    [Permission("查看租户", PermissionType.Button, 1, false, parentCode: Tenants)]
    public const string TenantsRead = "System.Tenants.Read";

    [Permission("创建租户", PermissionType.Button, 2, false, parentCode: Tenants)]
    public const string TenantsCreate = "System.Tenants.Create";

    [Permission("编辑租户", PermissionType.Button, 3, false, parentCode: Tenants)]
    public const string TenantsUpdate = "System.Tenants.Update";

    [Permission("删除租户", PermissionType.Button, 4, false, parentCode: Tenants)]
    public const string TenantsDelete = "System.Tenants.Delete";

    [Permission("会话管理", PermissionType.Menu, 5, false, "/system/sessions", System)]
    public const string Sessions = "System.Sessions";

    [Permission("查看会话", PermissionType.Button, 1, false, parentCode: Sessions)]
    public const string SessionsRead = "System.Sessions.Read";

    [Permission("强制下线", PermissionType.Button, 2, false, parentCode: Sessions)]
    public const string SessionsRevoke = "System.Sessions.Revoke";

    [Permission("课程管理", PermissionType.Directory, 200, true, "/library")]
    public const string Library = "Library";

    [Permission("年级管理", PermissionType.Menu, 1, true, "/library/grades", Library)]
    public const string LibraryGrades = "Library.Grades";

    [Permission("查看年级", PermissionType.Button, 1, true, parentCode: LibraryGrades)]
    public const string LibraryGradesRead = "Library.Grades.Read";

    [Permission("创建年级", PermissionType.Button, 2, true, parentCode: LibraryGrades)]
    public const string LibraryGradesCreate = "Library.Grades.Create";

    [Permission("编辑年级", PermissionType.Button, 3, true, parentCode: LibraryGrades)]
    public const string LibraryGradesUpdate = "Library.Grades.Update";

    [Permission("删除年级", PermissionType.Button, 4, true, parentCode: LibraryGrades)]
    public const string LibraryGradesDelete = "Library.Grades.Delete";

    [Permission("专题管理", PermissionType.Menu, 2, true, "/library/topics", Library)]
    public const string LibraryTopics = "Library.Topics";

    [Permission("查看专题", PermissionType.Button, 1, true, parentCode: LibraryTopics)]
    public const string LibraryTopicsRead = "Library.Topics.Read";

    [Permission("创建专题", PermissionType.Button, 2, true, parentCode: LibraryTopics)]
    public const string LibraryTopicsCreate = "Library.Topics.Create";

    [Permission("编辑专题", PermissionType.Button, 3, true, parentCode: LibraryTopics)]
    public const string LibraryTopicsUpdate = "Library.Topics.Update";

    [Permission("删除专题", PermissionType.Button, 4, true, parentCode: LibraryTopics)]
    public const string LibraryTopicsDelete = "Library.Topics.Delete";

    public const string LibraryLessons = "Library.Lessons";

    [Permission("查看课节", PermissionType.Button, 1, true, parentCode: LibraryTopics)]
    public const string LibraryLessonsRead = "Library.Lessons.Read";

    [Permission("创建课节", PermissionType.Button, 2, true, parentCode: LibraryTopics)]
    public const string LibraryLessonsCreate = "Library.Lessons.Create";

    [Permission("编辑课节", PermissionType.Button, 3, true, parentCode: LibraryTopics)]
    public const string LibraryLessonsUpdate = "Library.Lessons.Update";

    [Permission("删除课节", PermissionType.Button, 4, true, parentCode: LibraryTopics)]
    public const string LibraryLessonsDelete = "Library.Lessons.Delete";

    [Permission("套餐管理", PermissionType.Menu, 6, true, "/admin/plans", System)]
    public const string Plans = "System.Plans";

    [Permission("查看套餐", PermissionType.Button, 1, true, parentCode: Plans)]
    public const string PlansRead = "System.Plans.Read";

    [Permission("创建套餐", PermissionType.Button, 2, true, parentCode: Plans)]
    public const string PlansCreate = "System.Plans.Create";

    [Permission("编辑套餐", PermissionType.Button, 3, true, parentCode: Plans)]
    public const string PlansUpdate = "System.Plans.Update";

    [Permission("删除套餐", PermissionType.Button, 4, true, parentCode: Plans)]
    public const string PlansDelete = "System.Plans.Delete";
}