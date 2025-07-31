using F.Fireworks.Api.Extensions;
using F.Fireworks.Application.Features.Account.Commands;
using F.Fireworks.Domain.Permissions;
using F.Fireworks.Infrastructure.Auth;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace F.Fireworks.Api.Features.Account;

public class ChangePwdEndpoint(IMediator mediator) : Endpoint<ChangePwdCommand>
{
    public override void Configure()
    {
        Post("me/change-password");
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        Policy(b => b.AddRequirements(new PermissionRequirement(PermissionDefinitions.UsersChangePassword)));
        Description(x => x.WithTags("Account"));
        Summary(s => s.Summary = "修改密码");
    }

    public override async Task HandleAsync(ChangePwdCommand req, CancellationToken ct)
    {
        var result = await mediator.Send(req, ct);
        await this.SendMyResultAsync(result, ct);
    }
}