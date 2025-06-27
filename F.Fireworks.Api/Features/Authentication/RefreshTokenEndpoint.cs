using F.Fireworks.Api.Extensions;
using F.Fireworks.Application.Features.Authentication.Commands;
using FastEndpoints;
using MediatR;

namespace F.Fireworks.Api.Features.Authentication;

public class RefreshTokenEndpoint(IMediator mediator) : Endpoint<RefreshTokenCommand, IResult>
{
    public override void Configure()
    {
        Post("auth/refresh");
        AllowAnonymous();
    }

    public override async Task HandleAsync(RefreshTokenCommand req, CancellationToken ct)
    {
        var result = await mediator.Send(req, ct);
        await SendAsync(result.ToMinimalApiResult(), cancellation: ct);
    }
}