using F.Fireworks.Api.Extensions;
using F.Fireworks.Application.Features.Roles.Commands;
using F.Fireworks.Domain.Permissions;
using F.Fireworks.Infrastructure.Auth;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace F.Fireworks.Api.Features.Roles;

public class DeleteRoleEndpoint(IMediator mediator) : Endpoint<DeleteRoleCommand, IResult>
{
    public override void Configure()
    {
        Delete("roles/{Id}");
        Description(x => x.WithTags("Roles"));
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        Policy(b => b.AddRequirements(new PermissionRequirement(PermissionDefinitions.RolesDelete)));
        Summary(s => s.Summary = "删除一个角色");
    }

    public override async Task HandleAsync(DeleteRoleCommand req, CancellationToken ct)
    {
        var result = await mediator.Send(req, ct);
        await SendAsync(result.ToMinimalApiResult(), cancellation: ct);
    }
}