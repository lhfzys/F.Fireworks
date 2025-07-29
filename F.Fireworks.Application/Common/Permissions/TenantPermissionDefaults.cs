using F.Fireworks.Domain.Permissions;

namespace F.Fireworks.Application.Common.Permissions;

public static class TenantPermissionDefaults
{
    public static readonly List<string> AdminPermissions =
    [
        PermissionDefinitions.Dashboard,
        PermissionDefinitions.Home,

        PermissionDefinitions.Users,
        PermissionDefinitions.UsersRead,
        PermissionDefinitions.UsersUpdate,
        PermissionDefinitions.UsersCreate,
        PermissionDefinitions.UsersDelete,
        PermissionDefinitions.UsersUpdateRoles,

        PermissionDefinitions.Roles,
        PermissionDefinitions.RolesRead,
        PermissionDefinitions.RolesCreate,
        PermissionDefinitions.RolesUpdate,
        PermissionDefinitions.RolesDelete,

        // 业务权限
        PermissionDefinitions.Library,
        PermissionDefinitions.LibraryGrades,
        PermissionDefinitions.LibraryGradesRead,
        PermissionDefinitions.LibraryGradesCreate,
        PermissionDefinitions.LibraryGradesUpdate,
        PermissionDefinitions.LibraryGradesDelete,

        PermissionDefinitions.LibraryTopics,
        PermissionDefinitions.LibraryTopicsRead,
        PermissionDefinitions.LibraryTopicsCreate,
        PermissionDefinitions.LibraryTopicsUpdate,
        PermissionDefinitions.LibraryTopicsDelete,
        PermissionDefinitions.LibraryLessons,
        PermissionDefinitions.LibraryLessonsRead,
        PermissionDefinitions.LibraryLessonsCreate,
        PermissionDefinitions.LibraryLessonsUpdate,
        PermissionDefinitions.LibraryLessonsDelete
    ];
}