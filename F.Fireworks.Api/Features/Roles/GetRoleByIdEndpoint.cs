using F.Fireworks.Api.Extensions;
using F.Fireworks.Application.Features.Roles.Queries;
using F.Fireworks.Domain.Permissions;
using F.Fireworks.Infrastructure.Auth;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace F.Fireworks.Api.Features.Roles;

public class GetRoleByIdEndpoint(IMediator mediator) : Endpoint<GetRoleByIdQuery>
{
    public override void Configure()
    {
        Get("roles/{Id:guid}");
        Description(x => x.WithTags("Roles"));
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        Policy(b => b.AddRequirements(new PermissionRequirement(PermissionDefinitions.RolesRead)));
        Summary(s => s.Summary = "获取单个角色详情信息");
    }

    public override async Task HandleAsync([AsParameters] GetRoleByIdQuery req, CancellationToken ct)
    {
        var result = await mediator.Send(req, ct);
        await this.SendMyResultAsync(result, ct);
    }
}