using Ardalis.Result;
using F.Fireworks.Application.Contracts.Persistence;
using F.Fireworks.Application.DTOs.Tenants;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace F.Fireworks.Application.Features.Tenants.Queries;

public class GetTenantByIdQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetTenantByIdQuery, Result<TenantDetailsDto>>
{
    public async Task<Result<TenantDetailsDto>> Handle(GetTenantByIdQuery request, CancellationToken cancellationToken)
    {
        var tenant = await context.Tenants
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

        if (tenant is null) return Result.NotFound("租户不存在或已被删除");
        var dto = new TenantDetailsDto(tenant.Id, tenant.Name, tenant.Type, tenant.IsActive, tenant.PlanId);
        return Result.Success(dto);
    }
}