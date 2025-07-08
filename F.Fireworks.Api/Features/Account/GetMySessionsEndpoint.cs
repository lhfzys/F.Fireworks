using F.Fireworks.Api.Extensions;
using F.Fireworks.Application.Features.Authentication.Queries;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace F.Fireworks.Api.Features.Account;

public class GetMySessionsEndpoint(IMediator mediator) : EndpointWithoutRequest<IResult>
{
    public override void Configure()
    {
        Get("account/sessions");
        Description(x => x.WithTags("Account"));

        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        Summary(s =>
        {
            s.Summary = "获取我的活跃登录会话";
            s.Description = "返回当前用户所有未过期的登录会话列表，可用于设备管理。";
        });
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var result = await mediator.Send(new GetMySessionsQuery(), ct);
        await this.SendMyResultAsync(result, ct);
    }
}