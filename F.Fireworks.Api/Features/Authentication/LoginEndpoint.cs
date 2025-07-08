using F.Fireworks.Api.Extensions;
using F.Fireworks.Application.DTOs.Authentication;
using FastEndpoints;
using MediatR;

namespace F.Fireworks.Api.Features.Authentication;

public class LoginEndpoint(IMediator mediator) : Endpoint<LoginDto.LoginCommand>
{
    public override void Configure()
    {
        Post("auth/login");
        AllowAnonymous();
        Description(x => x.WithTags("Auth"));
        Options(x => x.RequireRateLimiting("token"));
        Summary(s => s.Summary = "用户登录");
    }

    public override async Task HandleAsync(LoginDto.LoginCommand req, CancellationToken ct)
    {
        var result = await mediator.Send(req, ct);
        await this.SendMyResultAsync(result, ct);
    }
}