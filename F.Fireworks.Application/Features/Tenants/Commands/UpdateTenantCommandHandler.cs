using Ardalis.Result;
using F.Fireworks.Application.Contracts.Persistence;
using F.Fireworks.Domain.Constants;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace F.Fireworks.Application.Features.Tenants.Commands;

public class UpdateTenantCommandHandler(IApplicationDbContext context) : IRequestHandler<UpdateTenantCommand, Result>
{
    public async Task<Result> Handle(UpdateTenantCommand request, CancellationToken cancellationToken)
    {
        var tenant = await context.Tenants
            .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);
        if (tenant == null)
            return Result.NotFound("租户不存在或已被删除");
        if (tenant.Name == SystemConstants.SuperTenants) return Result.Forbidden("租户 'System' 不能被修改");
        tenant.Name = request.Name;
        tenant.IsActive = request.IsActive;
        tenant.PlanId = request.PlanId;
        await context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}