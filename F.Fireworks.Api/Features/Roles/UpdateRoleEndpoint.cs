using F.Fireworks.Api.Extensions;
using F.Fireworks.Application.Features.Roles.Commands;
using F.Fireworks.Domain.Permissions;
using F.Fireworks.Infrastructure.Auth;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace F.Fireworks.Api.Features.Roles;

public class UpdateRoleEndpoint(IMediator mediator) : Endpoint<UpdateRoleCommand, IResult>
{
    public override void Configure()
    {
        Put("roles/{Id}");
        Description(x => x.WithTags("Roles"));
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        Policy(b => b.AddRequirements(new PermissionRequirement(PermissionDefinitions.RolesUpdate)));
        Summary(s => s.Summary = "更新一个已存在的角色");
    }

    public override async Task HandleAsync(UpdateRoleCommand req, CancellationToken ct)
    {
        var result = await mediator.Send(req, ct);
        await SendAsync(result.ToMinimalApiResult(), cancellation: ct);
    }
}