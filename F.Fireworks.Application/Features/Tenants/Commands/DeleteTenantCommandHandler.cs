using Ardalis.Result;
using F.Fireworks.Application.Contracts.Persistence;
using MediatR;

namespace F.Fireworks.Application.Features.Tenants.Commands;

public class DeleteTenantCommandHandler(IApplicationDbContext context) : IRequestHandler<DeleteTenantCommand, Result>
{
    public async Task<Result> Handle(DeleteTenantCommand request, CancellationToken cancellationToken)
    {
        var tenant = await context.Tenants.FindAsync([request.Id], cancellationToken);
        if (tenant is null) return Result.NotFound("租户不存在或已被删除");

        context.Tenants.Remove(tenant);

        tenant.IsActive = false;
        await context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}