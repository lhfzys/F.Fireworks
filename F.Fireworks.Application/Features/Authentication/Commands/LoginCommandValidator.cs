using F.Fireworks.Application.DTOs.Authentication;
using FluentValidation;

namespace F.Fireworks.Application.Features.Authentication.Commands;

public class LoginCommandValidator : AbstractValidator<LoginDto.LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty();

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(6);
    }
}