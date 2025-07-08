using F.Fireworks.Api.Extensions;
using F.Fireworks.Application.Features.Account.Queries;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace F.Fireworks.Api.Features.Account;

public class GetMyProfileEndpoint(IMediator mediator) : EndpointWithoutRequest<IResult>
{
    public override void Configure()
    {
        Get("me/profile");
        Description(x => x.WithTags("Account"));
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        Summary(s => s.Summary = "获取当前用户的完整信息（含权限和菜单）");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var result = await mediator.Send(new GetMyProfileQuery(), ct);
        // await SendAsync(result.ToMinimalApiResult(), cancellation: ct);
        await HttpContext.WriteAsApiResponse(result);
    }
}