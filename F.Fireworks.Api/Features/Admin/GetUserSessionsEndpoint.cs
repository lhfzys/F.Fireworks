using F.Fireworks.Api.Extensions;
using F.Fireworks.Application.Features.Authentication.Queries;
using F.Fireworks.Domain.Permissions;
using F.Fireworks.Infrastructure.Auth;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace F.Fireworks.Api.Features.Admin;

public class GetUserSessionsEndpoint(IMediator mediator) : Endpoint<GetUserSessionsAsAdminQuery, IResult>
{
    public override void Configure()
    {
        Get("admin/users/{UserId}/sessions");
        Description(x => x.WithTags("Admin.Sessions"));
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        Policy(b => b.AddRequirements(new PermissionRequirement(PermissionDefinitions.SessionsRead)));
        Summary(s => s.Summary = "管理员获取指定用户的活跃会话列表");
    }

    public override async Task HandleAsync(GetUserSessionsAsAdminQuery req, CancellationToken ct)
    {
        var result = await mediator.Send(req, ct);
        await SendAsync(result.ToMinimalApiResult(), cancellation: ct);
    }
}