using FluentValidation;

namespace F.Fireworks.Application.Features.Authentication.Commands;

public class AdminLoginCommandValidator : AbstractValidator<AdminLoginCommand>
{
    public AdminLoginCommandValidator()
    {
        RuleFor(x => x.Identifier).NotEmpty();
        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(6);
    }
}