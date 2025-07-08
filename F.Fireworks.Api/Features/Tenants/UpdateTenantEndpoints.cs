using F.Fireworks.Api.Extensions;
using F.Fireworks.Application.Features.Tenants.Commands;
using F.Fireworks.Domain.Permissions;
using F.Fireworks.Infrastructure.Auth;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace F.Fireworks.Api.Features.Tenants;

public class UpdateTenantEndpoints(IMediator mediator) : Endpoint<UpdateTenantCommand>
{
    public override void Configure()
    {
        Put("tenants/{Id}");
        Description(x => x.WithTags("Tenants"));
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        Policy(b => b.AddRequirements(new PermissionRequirement(PermissionDefinitions.TenantsUpdate)));
        Summary(s => s.Summary = "更新一个已存在的租户");
    }

    public override async Task HandleAsync(UpdateTenantCommand req, CancellationToken ct)
    {
        var result = await mediator.Send(req, ct);
        await this.SendMyResultAsync(result, ct);
    }
}