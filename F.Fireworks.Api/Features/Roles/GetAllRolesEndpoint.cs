using F.Fireworks.Api.Extensions;
using F.Fireworks.Application.Features.Roles.Queries;
using F.Fireworks.Domain.Permissions;
using F.Fireworks.Infrastructure.Auth;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace F.Fireworks.Api.Features.Roles;

public class GetAllRolesEndpoint(IMediator mediator) : EndpointWithoutRequest<IResult>
{
    public override void Configure()
    {
        Get("roles");
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        Policy(b => b.AddRequirements(new PermissionRequirement(PermissionDefinitions.RolesRead)));
        Description(x => x.WithTags("Roles"));
        Summary(s => s.Summary = "获取所有角色列表");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var result = await mediator.Send(new GetAllRolesQuery(), ct);
        await this.SendMyResultAsync(result, ct);
    }
}