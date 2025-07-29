using Ardalis.Result;
using F.Fireworks.Application.Contracts.Persistence;
using F.Fireworks.Domain.Subscriptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace F.Fireworks.Application.Features.Plans.Commands;

public class UpdatePlanCommandHandler(IApplicationDbContext context) : IRequestHandler<UpdatePlanCommand, Result>
{
    public async Task<Result> Handle(UpdatePlanCommand request, CancellationToken cancellationToken)
    {
        var plan = await context.Plans
            .Include(p => p.Permissions)
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (plan is null) return Result.NotFound("套餐计划不存在或已被删除");

        plan.Name = request.Name;
        plan.Description = request.Description;
        plan.IsActive = request.IsActive;

        context.PlanPermissions.RemoveRange(plan.Permissions);
        var newPermissions = request.PermissionIds
            .Select(pId => new PlanPermission { PlanId = plan.Id, PermissionId = pId });
        await context.PlanPermissions.AddRangeAsync(newPermissions, cancellationToken);

        await context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}