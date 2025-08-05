using F.Fireworks.Api.Extensions;
using F.Fireworks.Application.Features.Roles.Commands;
using F.Fireworks.Domain.Permissions;
using F.Fireworks.Infrastructure.Auth;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace F.Fireworks.Api.Features.Roles;

public class UpdateRolePermissionsEndpoint(IMediator mediator) : Endpoint<UpdateRolePermissionsCommand>
{
    public override void Configure()
    {
        Put("roles/{RoleId}/permissions");
        Description(x => x.WithTags("Roles"));
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        Policy(b => b.AddRequirements(new PermissionRequirement(PermissionDefinitions.RolesUpdate)));
        Summary(s => s.Summary = "更新指定角色的权限");
    }

    public override async Task HandleAsync(UpdateRolePermissionsCommand req,
        CancellationToken ct)
    {
        var result = await mediator.Send(req, ct);
        await this.SendMyResultAsync(result, ct);
    }
}