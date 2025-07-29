using Ardalis.Result;
using F.Fireworks.Application.Contracts.Persistence;
using F.Fireworks.Application.DTOs.Subscriptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace F.Fireworks.Application.Features.Plans.Queries;

public class GetPlanByIdQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetPlanByIdQuery, Result<PlanDetailsDto>>
{
    public async Task<Result<PlanDetailsDto>> Handle(GetPlanByIdQuery request, CancellationToken cancellationToken)
    {
        var plan = await context.Plans
            .Include(p => p.Permissions)
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (plan is null) return Result.NotFound("套餐计划不存在或已被删除");
        return Result.Success(new PlanDetailsDto(plan.Id, plan.Name, plan.Description, plan.IsActive,
            plan.Permissions.Select(p => p.PermissionId).ToList()));
    }
}