using F.Fireworks.Application.Contracts.Persistence;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace F.Fireworks.Application.Features.Tenants.Commands;

public class CreateTenantCommandValidator : AbstractValidator<CreateTenantCommand>
{
    public CreateTenantCommandValidator(IApplicationDbContext context)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Name)
            .MustAsync(async (name, ct) => !await context.Tenants.AnyAsync(x => x.Name == name, ct))
            .WithMessage("Tenant with name '{PropertyValue}' already exists.");
    }
}