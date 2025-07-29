using Ardalis.Result;
using F.Fireworks.Application.Contracts.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace F.Fireworks.Application.Features.Plans.Commands;

public class DeletePlanCommandHandler(IApplicationDbContext context) : IRequestHandler<DeletePlanCommand, Result>
{
    public async Task<Result> Handle(DeletePlanCommand request, CancellationToken cancellationToken)
    {
        var plan = await context.Plans
            .Include(p => p.Tenants)
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (plan is null) return Result.NotFound("套餐计划不存在或已被删除");

        if (plan.Tenants.Count != 0) return Result.Conflict("无法删除，有关联的租户");
        context.Plans.Remove(plan);
        await context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}