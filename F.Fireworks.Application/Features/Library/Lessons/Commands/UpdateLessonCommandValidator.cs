using F.Fireworks.Application.Contracts.Persistence;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace F.Fireworks.Application.Features.Library.Lessons.Commands;

public class UpdateLessonCommandValidator : AbstractValidator<UpdateLessonCommand>
{
    public UpdateLessonCommandValidator(IApplicationDbContext context)
    {
        RuleFor(x => x.Id).NotEmpty()
            .MustAsync(async (id, ct) => await context.LibraryLessons.AnyAsync(x => x.Id == id, ct))
            .WithMessage("课节不存在");
        RuleFor(x => x.Title).NotEmpty().MaximumLength(100);
        RuleFor(x => x)
            .MustAsync(async (command, ct) =>
                !await context.LibraryLessons.AnyAsync(x => x.Title == command.Title && x.Id != command.Id, ct))
            .WithMessage("课节 '{PropertyValue}' 已存在")
            .WithName("Title");
        RuleFor(x => x.Content).MaximumLength(1000);
        RuleFor(x => x.VideoUrl).MaximumLength(1000);
        RuleFor(x => x.DurationInMinutes).InclusiveBetween(0, 1000);
        RuleFor(x => x.SortOrder).InclusiveBetween(0, 1000);
        RuleFor(x => x.IsTrial).NotNull();
    }
}