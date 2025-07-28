using Ardalis.Result;
using F.Fireworks.Application.Contracts.Persistence;
using F.Fireworks.Domain.Subscriptions;
using MediatR;

namespace F.Fireworks.Application.Features.Plans.Commands;

public class CreatePlanCommandHandler(IApplicationDbContext context) : IRequestHandler<CreatePlanCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreatePlanCommand request, CancellationToken cancellationToken)
    {
        var plan = new Plan
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            IsActive = true
        };
        var planPermissions = request.PermissionIds
            .Select(pId => new PlanPermission { PlanId = plan.Id, PermissionId = pId })
            .ToList();
        await context.Plans.AddAsync(plan, cancellationToken);
        await context.PlanPermissions.AddRangeAsync(planPermissions, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success(plan.Id);
    }
}