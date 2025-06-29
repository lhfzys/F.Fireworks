using Ardalis.Result;
using F.Fireworks.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace F.Fireworks.Application.Features.Users.Commands;

public class UpdateUserRolesCommandHandler(UserManager<ApplicationUser> userManager)
    : IRequestHandler<UpdateUserRolesCommand, Result>
{
    public async Task<Result> Handle(UpdateUserRolesCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(request.UserId.ToString());
        if (user is null) return Result.NotFound("User not found.");

        // 获取用户当前的角色
        var currentRoles = await userManager.GetRolesAsync(user);

        // 计算需要添加的角色 (新角色列表有，但当前角色列表没有)
        var rolesToAdd = request.RoleNames.Except(currentRoles).ToList();
        if (rolesToAdd.Count != 0)
        {
            var addResult = await userManager.AddToRolesAsync(user, rolesToAdd);
            if (!addResult.Succeeded)
            {
                var errors = addResult.Errors
                    .Select(e => new ValidationError { Identifier = e.Code, ErrorMessage = e.Description })
                    .ToList();
                return Result.Invalid(errors);
            }
        }

        var rolesToRemove = currentRoles.Except(request.RoleNames).ToList();
        if (rolesToRemove.Any())
        {
            var removeResult = await userManager.RemoveFromRolesAsync(user, rolesToRemove);
            if (!removeResult.Succeeded)
            {
                var errors = removeResult.Errors
                    .Select(e => new ValidationError { Identifier = e.Code, ErrorMessage = e.Description })
                    .ToList();
                return Result.Invalid(errors);
            }
        }

        return Result.Success();
    }
}