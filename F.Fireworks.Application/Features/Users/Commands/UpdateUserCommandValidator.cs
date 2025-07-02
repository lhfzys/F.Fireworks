using F.Fireworks.Domain.Identity;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace F.Fireworks.Application.Features.Users.Commands;

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator(UserManager<ApplicationUser> userManager)
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .MustAsync(async (id, ct) => await userManager.FindByIdAsync(id.ToString()) is not null)
            .WithMessage("用户不存在");
        RuleFor(x => x.Status).IsInEnum();
    }
}