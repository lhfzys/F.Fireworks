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
        if (role is null) return Result.NotFound("角色不存在");
        if (!currentUser.IsInRole("SuperAdmin") && role.TenantId != currentUser.TenantId)
            return Result.Forbidden("禁止删除");
        if (role.Name != null)
        {
            var usersInRole = await userManager.GetUsersInRoleAsync(role.Name);
            if (usersInRole.Any())
                return Result.Error("当前角色已被使用，禁止删除");
        }

        var result = await roleManager.DeleteAsync(role);
        return !result.Succeeded
            ? Result.Invalid(result.Errors.Select(e => new ValidationError(e.Code, e.Description)).ToList())
            : Result.Success();
    }
}