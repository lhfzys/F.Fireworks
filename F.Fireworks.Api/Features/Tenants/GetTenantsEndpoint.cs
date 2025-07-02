using F.Fireworks.Api.Extensions;
using F.Fireworks.Application.DTOs.Tenants;
using F.Fireworks.Application.Features.Tenants.Queries;
using F.Fireworks.Domain.Permissions;
using F.Fireworks.Infrastructure.Auth;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace F.Fireworks.Api.Features.Tenants;

public class GetTenantsEndpoint(IMediator mediator) : Endpoint<TenantFilter, IResult>
{
    public override void Configure()
    {
        Get("tenants");
        Description(x => x.WithTags("Tenants"));
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        Policy(b => b.AddRequirements(new PermissionRequirement(PermissionDefinitions.TenantsRead)));
        Summary(s => s.Summary = "获取租户分页列表 (支持动态筛选和排序)");
    }

    public override async Task HandleAsync([AsParameters] TenantFilter req, CancellationToken ct)
    {
        var result = await mediator.Send(new GetTenantsQuery(req), ct);
        await SendAsync(result.ToMinimalApiResult(), cancellation: ct);
    }
}