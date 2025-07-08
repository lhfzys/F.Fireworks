using F.Fireworks.Api.Extensions;
using F.Fireworks.Application.Features.Authentication.Commands;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace F.Fireworks.Api.Features.Account;

public class RevokeSessionEndpoint(IMediator mediator) : Endpoint<RevokeSessionCommand>
{
    public override void Configure()
    {
        Delete("account/sessions/{SessionId}");
        Description(x => x.WithTags("Account"));
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);

        Summary(s =>
        {
            s.Summary = "吊销指定的登录会话";
            s.Description = "用于让用户从特定设备或浏览器上登出。";
        });
    }

    public override async Task HandleAsync(RevokeSessionCommand req, CancellationToken ct)
    {
        var result = await mediator.Send(req, ct);
        await this.SendMyResultAsync(result, ct);
    }
}