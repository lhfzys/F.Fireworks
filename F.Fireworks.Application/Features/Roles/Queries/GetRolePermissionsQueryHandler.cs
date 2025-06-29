using Ardalis.Result;
using F.Fireworks.Application.Contracts.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace F.Fireworks.Application.Features.Roles.Queries;

public class GetRolePermissionsQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetRolePermissionsQuery, Result<List<Guid>>>
{
    public async Task<Result<List<Guid>>> Handle(GetRolePermissionsQuery request, CancellationToken cancellationToken)
    {
        if (!await context.Roles.AnyAsync(r => r.Id == request.RoleId, cancellationToken))
            return Result<List<Guid>>.NotFound("Role not found.");
        var permissionIds = await context.RolePermissions
            .Where(rp => rp.RoleId == request.RoleId)
            .Select(rp => rp.PermissionId)
            .ToListAsync(cancellationToken);
        return Result<List<Guid>>.Success(permissionIds);
    }
}