using Ardalis.Result;
using F.Fireworks.Application.Contracts.Persistence;
using F.Fireworks.Domain.Tenants;
using MediatR;

namespace F.Fireworks.Application.Features.Tenants.Commands;

public class CreateTenantCommandHandler(IApplicationDbContext context)
    : IRequestHandler<CreateTenantCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateTenantCommand request, CancellationToken cancellationToken)
    {
        var newTenant = new Tenant
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Type = request.Type,
            IsActive = true,
            PlanId = request.PlanId
        };
        context.Tenants.Add(newTenant);
        await context.SaveChangesAsync(cancellationToken);
        return Result.Success(newTenant.Id);
    }
}