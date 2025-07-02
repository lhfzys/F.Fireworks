using Ardalis.Result;
using F.Fireworks.Application.Contracts.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace F.Fireworks.Application.Features.Tenants.Commands;

public class DeleteTenantCommandHandler(IApplicationDbContext context) : IRequestHandler<DeleteTenantCommand, Result>
{
    public async Task<Result> Handle(DeleteTenantCommand request, CancellationToken cancellationToken)
    {
        var tenant = await context.Tenants.FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);
        if (tenant is null) return Result.NotFound("租户不存在或已被删除");
        var hasUsers = await context.Users.AnyAsync(u => u.TenantId == request.Id, cancellationToken);
        if (hasUsers) return Result.Error("无法删除租户，当前租户下有成员！");
        context.Tenants.Remove(tenant);
        await context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}