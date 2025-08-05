using Ardalis.Result;
using F.Fireworks.Application.Contracts.Services;
using F.Fireworks.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace F.Fireworks.Application.Features.Roles.Commands;

public class DeleteRoleCommandHandler(
    RoleManager<ApplicationRole> roleManager,
    UserManager<ApplicationUser> userManager,
    ICurrentUserService currentUser)
    : IRequestHandler<DeleteRoleCommand, Result>
{
    public async Task<Result> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await roleManager.FindByIdAsync(request.Id.ToString());
        if (role is null || (!currentUser.IsInRole("SuperAdmin") && role.TenantId != currentUser.TenantId))
            return Result.NotFound("角色不存在或已被删除");
        if (role.Name != null)
        {
            var usersInRole = await userManager.GetUsersInRoleAsync(role.Name);
            if (usersInRole.Any())
                return Result.Error("当前角色已被使用，无法删除");
        }

        if (role.IsProtected) return Result.Forbidden("默认角色禁止变更");
        var result = await roleManager.DeleteAsync(role);
        return !result.Succeeded
            ? Result.Invalid(result.Errors.Select(e => new ValidationError(e.Code, e.Description)).ToList())
            : Result.Success();
    }
}