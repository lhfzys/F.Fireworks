using F.Fireworks.Api.Extensions;
using F.Fireworks.Application.Features.Tenants.Commands;
using F.Fireworks.Domain.Permissions;
using F.Fireworks.Infrastructure.Auth;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace F.Fireworks.Api.Features.Tenants;

public class PurgeTenantEndpoint(IMediator mediator) : Endpoint<PurgeTenantCommand>
{
    public override void Configure()
    {
        Delete("admin/tenants/{TenantId:guid}/purge");
        Description(x => x.WithTags("Tenants"));
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        Policy(b => b.AddRequirements(new PermissionRequirement(PermissionDefinitions.TenantsPurge)));
        Summary(s => s.Summary = "彻底清除一个租户及其所有数据");
    }

    public override async Task HandleAsync(PurgeTenantCommand req, CancellationToken ct)
    {
        var result = await mediator.Send(req, ct);
        await this.SendMyResultAsync(result, ct);
    }
}