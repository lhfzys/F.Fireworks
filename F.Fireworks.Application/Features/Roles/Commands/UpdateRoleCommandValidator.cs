using F.Fireworks.Domain.Identity;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace F.Fireworks.Application.Features.Roles.Commands;

public class UpdateRoleCommandValidator : AbstractValidator<UpdateRoleCommand>
{
    public UpdateRoleCommandValidator(RoleManager<ApplicationRole> roleManager)
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);

        // RuleFor(x => x)
        //     .MustAsync(async (command, ct) =>
        //     {
        //         var existingRole = await roleManager.FindByNameAsync(command.Name);
        //         return existingRole == null || existingRole.Id == command.Id;
        //     })
        //     .WithMessage("角色名 '{PropertyValue}' 已存在")
        //     .WithName("Name");
    }
}