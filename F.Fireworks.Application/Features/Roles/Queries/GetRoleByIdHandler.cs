using Ardalis.Result;
using F.Fireworks.Application.Contracts.Persistence;
using F.Fireworks.Application.Contracts.Services;
using F.Fireworks.Application.DTOs.Roles;
using F.Fireworks.Domain.Constants;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace F.Fireworks.Application.Features.Roles.Queries;

public class GetRoleByIdHandler(IApplicationDbContext context, ICurrentUserService currentUser)
    : IRequestHandler<GetRoleByIdQuery, Result<RoleDetailsDto>>
{
    public async Task<Result<RoleDetailsDto>> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
    {
        var query = context.Roles.AsNoTracking().Where(r => r.Id == request.Id);
        if (!currentUser.IsInRole(RoleConstants.SuperAdmin))
            query = query.Where(r => r.TenantId == currentUser.TenantId);

        var roleDetails = await query
            .Select(role => new RoleDetailsDto(
                role.Id,
                role.Name,
                role.Description,
                role.CreatedOn,
                role.TenantId,
                role.Tenant.Name,
                role.Permissions.Select(rp => rp.PermissionId).ToList(),
                role.IsProtected
            ))
            .FirstOrDefaultAsync(cancellationToken);

        return roleDetails is not null
            ? Result.Success(roleDetails)
            : Result.NotFound("角色不存在或您无权访问。");
    }
}