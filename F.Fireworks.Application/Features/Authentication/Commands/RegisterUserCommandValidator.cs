using F.Fireworks.Domain.Identity;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace F.Fireworks.Application.Features.Authentication.Commands;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator(UserManager<ApplicationUser> userManager)
    {
        RuleFor(x => x.UserName)
            .NotEmpty();

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(6);

        // 业务规则验证：检查用户名是否已存在
        RuleFor(x => x.UserName)
            .MustAsync(async (userName, cancellationToken) =>
                await userManager.FindByNameAsync(userName) is null)
            .WithMessage("Username already exists.");
    }
}