using F.Fireworks.Api.Extensions;
using F.Fireworks.Application.Features.Users.Commands;
using F.Fireworks.Domain.Permissions;
using F.Fireworks.Infrastructure.Auth;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace F.Fireworks.Api.Features.Users;

public class UpdateUserEndpoint(IMediator mediator) : Endpoint<UpdateUserCommand, IResult>
{
    public override void Configure()
    {
        Put("users/{Id}");
        Description(x => x.WithTags("Users"));
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        Policy(b => b.AddRequirements(new PermissionRequirement(PermissionDefinitions.UsersUpdate)));
        Summary(s => s.Summary = "更新用户信息");
    }

    public override async Task HandleAsync(UpdateUserCommand req, CancellationToken ct)
    {
        var result = await mediator.Send(req, ct);
        await SendAsync(result.ToMinimalApiResult(), cancellation: ct);
    }
}