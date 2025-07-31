using FluentValidation;

namespace F.Fireworks.Application.Features.Users.Commands;

public class ResetPasswordValidator : AbstractValidator<ResetPasswordCommand>
{
    public ResetPasswordValidator()
    {
        RuleFor(x => x.NewPassword).NotEmpty().MinimumLength(6).WithMessage("不能少于6位");
    }
}