using F.Fireworks.Api.Extensions;
using F.Fireworks.Application.Features.Authentication.Commands;
using F.Fireworks.Domain.Permissions;
using F.Fireworks.Infrastructure.Auth;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace F.Fireworks.Api.Features.Admin;

public class RevokeSessionEndpoint(IMediator mediator) : Endpoint<RevokeSessionAsAdminCommand>
{
    public override void Configure()
    {
        Delete("admin/sessions/{SessionId}");
        Description(x => x.WithTags("Admin.Sessions"));
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        Policy(b => b.AddRequirements(new PermissionRequirement(PermissionDefinitions.SessionsRevoke)));
        Summary(s => s.Summary = "管理员吊销指定会话（强制下线）");
    }

    public override async Task HandleAsync(RevokeSessionAsAdminCommand req, CancellationToken ct)
    {
        var result = await mediator.Send(req, ct);
        await this.SendMyResultAsync(result, ct);
    }
}