using F.Fireworks.Api.Extensions;
using F.Fireworks.Application.Features.Users.Commands;
using F.Fireworks.Domain.Permissions;
using F.Fireworks.Infrastructure.Auth;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace F.Fireworks.Api.Features.Users;

public class UpdateUserRolesEndpoint(IMediator mediator) : Endpoint<UpdateUserRolesCommand>
{
    public override void Configure()
    {
        Put("users/{UserId}/roles");
        Description(x => x.WithTags("Users"));
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        Policy(b => b.AddRequirements(new PermissionRequirement(PermissionDefinitions.UsersUpdateRoles)));
        Summary(s => s.Summary = "更新用户的角色分配");
    }

    public override async Task HandleAsync(UpdateUserRolesCommand req, CancellationToken ct)
    {
        var result = await mediator.Send(req, ct);
        await this.SendMyResultAsync(result, ct);
    }
}