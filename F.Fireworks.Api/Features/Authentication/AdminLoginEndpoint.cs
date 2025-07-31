using F.Fireworks.Api.Extensions;
using F.Fireworks.Application.Features.Authentication.Commands;
using FastEndpoints;
using MediatR;

namespace F.Fireworks.Api.Features.Authentication;

public class AdminLoginEndpoint(IMediator mediator) : Endpoint<AdminLoginCommand>
{
    public override void Configure()
    {
        Post("admin/login");
        AllowAnonymous();
        Description(x => x.WithTags("Auth"));
        Options(x => x.RequireRateLimiting("token"));
        Summary(s => s.Summary = "超管登录");
    }

    public override async Task HandleAsync(AdminLoginCommand req, CancellationToken ct)
    {
        var result = await mediator.Send(req, ct);
        await this.SendMyResultAsync(result, ct);
    }
}