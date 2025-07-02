using F.Fireworks.Api.Extensions;
using F.Fireworks.Application.Features.Users.Commands;
using F.Fireworks.Domain.Permissions;
using F.Fireworks.Infrastructure.Auth;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace F.Fireworks.Api.Features.Users;

public class DeleteUserEndpoint(IMediator mediator) : Endpoint<DeleteUserCommand, IResult>
{
    public override void Configure()
    {
        Delete("users/{Id}");
        Description(x => x.WithTags("Users"));
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        Policy(b => b.AddRequirements(new PermissionRequirement(PermissionDefinitions.UsersDelete)));
        Summary(s => s.Summary = "删除一个用户（软删除）");
    }

    public override async Task HandleAsync(DeleteUserCommand req, CancellationToken ct)
    {
        var result = await mediator.Send(req, ct);
        await SendAsync(result.ToMinimalApiResult(), cancellation: ct);
    }
}