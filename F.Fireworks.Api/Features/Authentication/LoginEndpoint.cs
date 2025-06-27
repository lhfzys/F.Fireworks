using F.Fireworks.Api.Extensions;
using F.Fireworks.Application.DTOs.Authentication;
using FastEndpoints;
using MediatR;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace F.Fireworks.Api.Features.Authentication;

public class LoginEndpoint(IMediator mediator) : Endpoint<LoginDto.LoginCommand, IResult>
{
    public override void Configure()
    {
        Post("auth/login");
        AllowAnonymous();
    }

    public override async Task HandleAsync(LoginDto.LoginCommand req, CancellationToken ct)
    {
        var result = await mediator.Send(req, ct);
        await SendAsync(result.ToMinimalApiResult(), cancellation: ct);
    }
}