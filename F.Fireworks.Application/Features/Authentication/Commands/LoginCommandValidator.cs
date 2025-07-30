using FluentValidation;

namespace F.Fireworks.Application.Features.Authentication.Commands;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Identifier)
            .NotEmpty();
        RuleFor(x => x.TenantIdentifier).NotEmpty();
        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(6);
    }
}