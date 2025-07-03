using F.Fireworks.Application.Contracts.Persistence;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace F.Fireworks.Application.Features.Library.Topics.Commands;

public class UpdateTopicCommandValidator : AbstractValidator<UpdateTopicCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateTopicCommandValidator(IApplicationDbContext context)
    {
        _context = context;
        RuleFor(x => x.Id).NotEmpty()
            .MustAsync(async (id, ct) => await context.LibraryTopics.AnyAsync(x => x.Id == id, ct))
            .WithMessage("专题不存在");

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Name)
            .MustAsync(BeUniqueNameAsync)
            .WithMessage("专题 '{PropertyValue}' 已存在");

        RuleFor(x => x.SortOrder).GreaterThanOrEqualTo(0);
    }

    private async Task<bool> BeUniqueNameAsync(
        UpdateTopicCommand command,
        string name,
        CancellationToken ct)
    {
        return !await _context.LibraryTopics
            .AnyAsync(t => t.Name == name && t.Id != command.Id, ct);
    }
}