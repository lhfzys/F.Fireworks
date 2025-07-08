using F.Fireworks.Api.Extensions;
using F.Fireworks.Application.Features.Authentication.Commands;
using FastEndpoints;
using MediatR;

namespace F.Fireworks.Api.Features.Authentication;

public class LogoutEndpoint(IMediator mediator) : Endpoint<LogoutCommand>
{
    public override void Configure()
    {
        Post("auth/logout");
        AllowAnonymous();
        Description(x => x.WithTags("Auth"));
        Summary(s => s.Summary = "用户登出");
    }

    public override async Task HandleAsync(LogoutCommand req, CancellationToken ct)
    {
        var result = await mediator.Send(req, ct);
        await this.SendMyResultAsync(result, ct);
    }
}