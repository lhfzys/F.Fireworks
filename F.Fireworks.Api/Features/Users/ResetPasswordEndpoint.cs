using F.Fireworks.Api.Extensions;
using F.Fireworks.Application.Features.Users.Commands;
using F.Fireworks.Domain.Permissions;
using F.Fireworks.Infrastructure.Auth;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace F.Fireworks.Api.Features.Users;

public class ResetPasswordEndpoint(IMediator mediator) : Endpoint<ResetPasswordCommand>
{
    public override void Configure()
    {
        Post("admin/users/{Id:guid}/reset-password");
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        Policy(b => b.AddRequirements(new PermissionRequirement(PermissionDefinitions.UsersResetPassword)));
        Description(x => x.WithTags("Admin"));
        Summary(s => s.Summary = "重置密码");
    }

    public override async Task HandleAsync(ResetPasswordCommand req, CancellationToken ct)
    {
        var result = await mediator.Send(req, ct);
        await this.SendMyResultAsync(result, ct);
    }
}