using F.Fireworks.Api.Extensions;
using F.Fireworks.Application.Features.Authentication.Commands;
using FastEndpoints;
using MediatR;

namespace F.Fireworks.Api.Features.Authentication;

public class RegisterEndpoint(IMediator mediator) : Endpoint<RegisterUserCommand>
{
    public override void Configure()
    {
        Post("auth/register");
        AllowAnonymous();
        Description(x => x.WithTags("Auth"));
        Summary(s => s.Summary = "用户注册");
    }

    public override async Task HandleAsync(RegisterUserCommand req, CancellationToken ct)
    {
        var result = await mediator.Send(req, ct);
        await this.SendMyResultAsync(result, ct);
    }
}