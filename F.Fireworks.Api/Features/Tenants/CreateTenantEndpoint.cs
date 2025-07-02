using F.Fireworks.Api.Extensions;
using F.Fireworks.Application.Features.Tenants.Commands;
using F.Fireworks.Domain.Permissions;
using F.Fireworks.Infrastructure.Auth;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace F.Fireworks.Api.Features.Tenants;

public class CreateTenantEndpoint(IMediator mediator) : Endpoint<CreateTenantCommand, IResult>
{
    public override void Configure()
    {
        Post("tenants");
        Description(x => x.WithTags("Tenants"));
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        Policy(b => b.AddRequirements(new PermissionRequirement(PermissionDefinitions.TenantsCreate)));
        Summary(s => s.Summary = "新建一个租户");
    }

    public override async Task HandleAsync(CreateTenantCommand req, CancellationToken ct)
    {
        var result = await mediator.Send(req, ct);
        await SendAsync(result.ToMinimalApiResult(), cancellation: ct);
    }
}