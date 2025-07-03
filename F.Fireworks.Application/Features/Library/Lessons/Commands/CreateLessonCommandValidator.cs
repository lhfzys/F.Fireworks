using F.Fireworks.Application.Contracts.Persistence;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace F.Fireworks.Application.Features.Library.Lessons.Commands;

public class CreateLessonCommandValidator : AbstractValidator<CreateLessonCommand>
{
    public CreateLessonCommandValidator(IApplicationDbContext context)
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("标题不能为空")
            .MaximumLength(200);

        RuleFor(x => x.LibraryTopicId)
            .NotEmpty()
            .MustAsync(async (topicId, ct) => await context.LibraryTopics.AnyAsync(g => g.Id == topicId, ct))
            .WithMessage("隶属专题不存在");

        // 验证在同一个专题下，课节名是否唯一
        RuleFor(x => x)
            .MustAsync(async (command, ct) =>
                !await context.LibraryLessons.AnyAsync(
                    t => t.Title == command.Title && t.LibraryTopicId == command.LibraryTopicId, ct))
            .WithMessage("同一专题下课节名不唯一")
            .WithName("Title");
    }
}