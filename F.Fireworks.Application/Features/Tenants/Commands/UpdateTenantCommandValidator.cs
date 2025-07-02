using F.Fireworks.Application.Contracts.Persistence;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace F.Fireworks.Application.Features.Tenants.Commands;

public class UpdateTenantCommandValidator : AbstractValidator<UpdateTenantCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateTenantCommandValidator(IApplicationDbContext context)
    {
        _context = context;
        RuleFor(x => x.Id).NotEmpty()
            .MustAsync(async (id, ct) => await context.Tenants.AnyAsync(x => x.Id == id, ct))
            .WithMessage("租户不存在");

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Name)
            .MustAsync(BeUniqueNameAsync)
            .WithMessage("Tenant with name '{PropertyValue}' already exists.");
    }

    private async Task<bool> BeUniqueNameAsync(
        UpdateTenantCommand command,
        string name,
        CancellationToken ct)
    {
        return !await _context.Tenants
            .AnyAsync(t => t.Name == name && t.Id != command.Id, ct);
    }
}