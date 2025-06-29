using Ardalis.Result;
using F.Fireworks.Application.Contracts.Persistence;
using F.Fireworks.Domain.Permissions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace F.Fireworks.Application.Features.Roles.Commands;

public class UpdateRolePermissionsCommandHandler(IApplicationDbContext context)
    : IRequestHandler<UpdateRolePermissionsCommand, Result>
{
    public async Task<Result> Handle(UpdateRolePermissionsCommand request, CancellationToken cancellationToken)
    {
        var role = await context.Roles.FindAsync([request.RoleId], cancellationToken);
        if (role is null) return Result.NotFound("Role not found.");
        var currentPermissions = await context.RolePermissions
            .Where(rp => rp.RoleId == request.RoleId)
            .ToListAsync(cancellationToken);

        context.RolePermissions.RemoveRange(currentPermissions);

        var newPermissions = request.PermissionIds
            .Select(permissionId => new ApplicationRolePermission
            {
                RoleId = request.RoleId,
                PermissionId = permissionId
            }).ToList();

        await context.RolePermissions.AddRangeAsync(newPermissions, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}