using FluentValidation;

namespace F.Fireworks.Application.Features.Users.Commands;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.UserName).NotEmpty().MinimumLength(6).WithMessage("用户名长度不能小于6");
        RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("用户邮箱格式不正确");
        RuleFor(x => x.Password).NotEmpty().MinimumLength(6).WithMessage("密码长度不能小于6");
    }
}