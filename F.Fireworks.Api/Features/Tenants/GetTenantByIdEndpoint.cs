using F.Fireworks.Api.Extensions;
using F.Fireworks.Application.Features.Tenants.Queries;
using F.Fireworks.Domain.Permissions;
using F.Fireworks.Infrastructure.Auth;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace F.Fireworks.Api.Features.Tenants;

public class GetTenantByIdEndpoint(IMediator mediator) : Endpoint<GetTenantByIdQuery>
{
    public override void Configure()
    {
        Get("tenants/{Id:guid}");
        Description(x => x.WithTags("Tenants"));
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        Policy(b => b.AddRequirements(new PermissionRequirement(PermissionDefinitions.TenantsRead)));
        Summary(s => s.Summary = "获取单个租户详情信息");
    }

    public override async Task HandleAsync([AsParameters] GetTenantByIdQuery req, CancellationToken ct)
    {
        var result = await mediator.Send(req, ct);
        await this.SendMyResultAsync(result, ct);
    }
}