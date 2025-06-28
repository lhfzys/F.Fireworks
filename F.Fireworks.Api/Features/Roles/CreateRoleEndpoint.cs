using F.Fireworks.Api.Extensions;
using F.Fireworks.Application.Features.Roles.Commands;
using F.Fireworks.Domain.Permissions;
using F.Fireworks.Infrastructure.Auth;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace F.Fireworks.Api.Features.Roles;

public class CreateRoleEndpoint(IMediator mediator) : Endpoint<CreateRoleCommand, IResult>
{
    public override void Configure()
    {
        Post("/api/roles");
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        Policy(b => b.AddRequirements(new PermissionRequirement(PermissionDefinitions.RolesCreate)));
        Tags("Roles");
        Description(x => x.WithTags("Roles"));
        Summary(s => s.Summary = "创建一个新角色");
    }

    public override async Task HandleAsync(CreateRoleCommand req, CancellationToken ct)
    {
        var result = await mediator.Send(req, ct);
        await SendAsync(result.ToMinimalApiResult(), cancellation: ct);
    }
}