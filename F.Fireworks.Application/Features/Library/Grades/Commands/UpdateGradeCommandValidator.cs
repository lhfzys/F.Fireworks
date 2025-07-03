using F.Fireworks.Application.Contracts.Persistence;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace F.Fireworks.Application.Features.Library.Grades.Commands;

public class UpdateGradeCommandValidator : AbstractValidator<UpdateGradeCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateGradeCommandValidator(IApplicationDbContext context)
    {
        _context = context;
        RuleFor(x => x.Id).NotEmpty()
            .MustAsync(async (id, ct) => await context.Grades.AnyAsync(x => x.Id == id, ct))
            .WithMessage("年级/级别不存在");

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Name)
            .MustAsync(BeUniqueNameAsync)
            .WithMessage("年级/级别 '{PropertyValue}' 已存在");

        RuleFor(x => x.SortOrder).GreaterThanOrEqualTo(0);
    }

    private async Task<bool> BeUniqueNameAsync(
        UpdateGradeCommand command,
        string name,
        CancellationToken ct)
    {
        return !await _context.Grades
            .AnyAsync(t => t.Name == name && t.Id != command.Id, ct);
    }
}