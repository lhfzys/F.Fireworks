using FluentValidation;

namespace F.Fireworks.Application.Features.Account.Commands;

public class ChangePwdCommandValidator : AbstractValidator<ChangePwdCommand>
{
    public ChangePwdCommandValidator()
    {
        RuleFor(x => x.OldPassword).NotEmpty().MinimumLength(6).WithMessage("不能小于6位");
        RuleFor(x => x.NewPassword)
            .NotEmpty()
            .MinimumLength(6).WithMessage("新密码长度不能少于6位。");
        RuleFor(x => x.ConfirmedPassword)
            .Equal(x => x.NewPassword)
            .WithMessage("两次输入的密码不一致。");
    }
}