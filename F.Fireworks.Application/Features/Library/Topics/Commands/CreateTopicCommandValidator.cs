using F.Fireworks.Application.Contracts.Persistence;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace F.Fireworks.Application.Features.Library.Topics.Commands;

public class CreateTopicCommandValidator : AbstractValidator<CreateTopicCommand>
{
    public CreateTopicCommandValidator(IApplicationDbContext context)
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("专题名称不能为空")
            .MaximumLength(200);

        RuleFor(x => x.GradeId)
            .NotEmpty()
            .MustAsync(async (gradeId, ct) => await context.Grades.AnyAsync(g => g.Id == gradeId, ct))
            .WithMessage("隶属年级不存在");

        // 验证在同一个年级下，专题名是否唯一
        RuleFor(x => x)
            .MustAsync(async (command, ct) =>
                !await context.LibraryTopics.AnyAsync(t => t.Name == command.Name && t.GradeId == command.GradeId, ct))
            .WithMessage("同一年级下专题名称不唯一")
            .WithName("Name");
    }
}