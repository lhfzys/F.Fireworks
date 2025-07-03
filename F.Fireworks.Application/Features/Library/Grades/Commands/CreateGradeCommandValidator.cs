using F.Fireworks.Application.Contracts.Persistence;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace F.Fireworks.Application.Features.Library.Grades.Commands;

public class CreateGradeCommandValidator : AbstractValidator<CreateGradeCommand>
{
    public CreateGradeCommandValidator(IApplicationDbContext context)
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("年级/级别不能为空")
            .MaximumLength(100)
            .MustAsync(async (name, ct) => !await context.Grades.AnyAsync(g => g.Name == name, ct))
            .WithMessage("年级/级别名称已存在");

        RuleFor(x => x.SortOrder).GreaterThanOrEqualTo(0);
    }
}