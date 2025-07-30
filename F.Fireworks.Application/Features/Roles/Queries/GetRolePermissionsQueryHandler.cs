using Ardalis.Result;
using F.Fireworks.Application.Contracts.Persistence;
using F.Fireworks.Application.Contracts.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace F.Fireworks.Application.Features.Roles.Queries;

public class GetRolePermissionsQueryHandler(IApplicationDbContext context, ICurrentUserService currentUser)
    : IRequestHandler<GetRolePermissionsQuery, Result<List<Guid>>>
{
    public async Task<Result<List<Guid>>> Handle(GetRolePermissionsQuery request, CancellationToken cancellationToken)
    {
        var role = await context.Roles.FirstOrDefaultAsync(r => r.Id == request.RoleId, cancellationToken);
        if (role is null || (!currentUser.IsInRole("SuperAdmin") && role.TenantId != currentUser.TenantId))
            return Result.NotFound("角色不存在或已被删除");
        var permissionIds = await context.RolePermissions
            .Where(rp => rp.RoleId == request.RoleId)
            .Select(rp => rp.PermissionId)
            .ToListAsync(cancellationToken);
        return Result<List<Guid>>.Success(permissionIds);
    }
}