using F.Fireworks.Domain.Identity;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace F.Fireworks.Application.Features.Users.Commands;

public class UpdateUserRolesCommandValidator : AbstractValidator<UpdateUserRolesCommand>
{
    public UpdateUserRolesCommandValidator(UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager)
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .MustAsync(async (id, ct) => await userManager.FindByIdAsync(id.ToString()) is not null)
            .WithMessage("User not found.");

        // RuleFor(x => x.RoleNames)
        //     .NotNull()
        //     .MustAsync(async (roleNames, ct) =>
        //     {
        //         foreach (var roleName in roleNames)
        //             if (!await roleManager.RoleExistsAsync(roleName))
        //                 return false;
        //         return true;
        //     }).WithMessage("One or more roles do not exist.");
    }
}