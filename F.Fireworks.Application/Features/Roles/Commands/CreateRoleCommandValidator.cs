using F.Fireworks.Domain.Identity;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace F.Fireworks.Application.Features.Roles.Commands;

public class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
{
    public CreateRoleCommandValidator(RoleManager<ApplicationRole> roleManager)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);

        // 验证角色名是否唯一
        // RuleFor(x => x.Name)
        //     .MustAsync(async (name, ct) => !await roleManager.RoleExistsAsync(name))
        //     .WithMessage("Role with name '{PropertyValue}' already exists.");
    }
}