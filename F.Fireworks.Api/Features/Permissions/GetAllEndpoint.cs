using F.Fireworks.Api.Extensions;
using F.Fireworks.Application.Features.Permissions.Queries;
using F.Fireworks.Domain.Permissions;
using F.Fireworks.Infrastructure.Auth;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace F.Fireworks.Api.Features.Permissions;

public class GetAllEndpoint(IMediator mediator) : EndpointWithoutRequest<IResult>
{
    public override void Configure()
    {
        Get("permissions");

        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        Policy(b => b.AddRequirements(new PermissionRequirement(PermissionDefinitions.RolesRead)));

        Summary(s =>
        {
            s.Summary = "获取所有权限的树状列表";
            s.Description = "用于后台管理界面的权限树展示。";
        });
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var result = await mediator.Send(new GetAllPermissionsQuery(), ct);
        await SendAsync(result.ToMinimalApiResult(), cancellation: ct);
    }
}