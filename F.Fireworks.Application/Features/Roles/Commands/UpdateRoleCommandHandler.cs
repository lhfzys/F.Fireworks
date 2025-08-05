using Ardalis.Result;
using F.Fireworks.Application.Contracts.Services;
using F.Fireworks.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace F.Fireworks.Application.Features.Roles.Commands;

public class UpdateRoleCommandHandler(RoleManager<ApplicationRole> roleManager, ICurrentUserService currentUser)
    : IRequestHandler<UpdateRoleCommand, Result>
{
    public async Task<Result> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await roleManager.FindByIdAsync(request.Id.ToString());
        if (role is null || (!currentUser.IsInRole("SuperAdmin") && role.TenantId != currentUser.TenantId))
            return Result.NotFound("角色不存在或已被删除");
        if (role.IsProtected) return Result.Forbidden("默认角色禁止变更");
        role.Name = request.Name;
        role.Description = request.Description;
        var result = await roleManager.UpdateAsync(role);
        return !result.Succeeded
            ? Result.Invalid(result.Errors.Select(e => new ValidationError(e.Code, e.Description)).ToList())
            : Result.Success();
    }
}