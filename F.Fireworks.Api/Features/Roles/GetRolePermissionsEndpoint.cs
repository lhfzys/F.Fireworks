using F.Fireworks.Api.Extensions;
using F.Fireworks.Application.Features.Roles.Queries;
using F.Fireworks.Domain.Permissions;
using F.Fireworks.Infrastructure.Auth;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;

namespace F.Fireworks.Api.Features.Roles;

public class GetRolePermissionsEndpoint(IMediator mediator) : Endpoint<GetRolePermissionsQuery, IResult>
{
    public override void Configure()
    {
        Get("roles/{RoleId}/permissions");
        Description(x => x.WithTags("Roles"));
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        Policy(b => b.AddRequirements(new PermissionRequirement(PermissionDefinitions.RolesRead)));
        Summary(s => s.Summary = "获取指定角色的权限列表");
    }

    public override async Task HandleAsync([FromRoute] GetRolePermissionsQuery req, CancellationToken ct)
    {
        var result = await mediator.Send(req, ct);
        await SendAsync(result.ToMinimalApiResult(), cancellation: ct);
    }
}