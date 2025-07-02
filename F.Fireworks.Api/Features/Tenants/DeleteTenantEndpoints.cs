using F.Fireworks.Api.Extensions;
using F.Fireworks.Application.Features.Tenants.Commands;
using F.Fireworks.Domain.Permissions;
using F.Fireworks.Infrastructure.Auth;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace F.Fireworks.Api.Features.Tenants;

public class DeleteTenantEndpoints(IMediator mediator) : Endpoint<DeleteTenantCommand, IResult>
{
    public override void Configure()
    {
        Delete("tenants/{Id}");
        Description(x => x.WithTags("Tenants"));
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        Policy(b => b.AddRequirements(new PermissionRequirement(PermissionDefinitions.TenantsDelete)));
        Summary(s => s.Summary = "删除一个租户（软删除）");
    }

    public override async Task HandleAsync(DeleteTenantCommand req, CancellationToken ct)
    {
        var result = await mediator.Send(req, ct);
        await SendAsync(result.ToMinimalApiResult(), cancellation: ct);
    }
}