using F.Fireworks.Api.Extensions;
using F.Fireworks.Application.DTOs.Users;
using F.Fireworks.Application.Features.Users.Queries;
using F.Fireworks.Domain.Permissions;
using F.Fireworks.Infrastructure.Auth;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace F.Fireworks.Api.Features.Users;

public class GetUsersEndpoint(IMediator mediator) : Endpoint<UserFilter, IResult>
{
    public override void Configure()
    {
        Get("users");
        Description(x => x.WithTags("Users"));
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        Policy(b => b.AddRequirements(new PermissionRequirement(PermissionDefinitions.UsersRead)));
        Summary(s => s.Summary = "获取用户分页列表 (支持动态筛选和排序)");
    }

    public override async Task HandleAsync([AsParameters] UserFilter req, CancellationToken ct)
    {
        var query = new GetUsersQuery(req);
        var result = await mediator.Send(query, ct);
        await SendAsync(result.ToMinimalApiResult(), cancellation: ct);
    }
}