using F.Fireworks.Api.Extensions;
using F.Fireworks.Application.Features.Users.Commands;
using F.Fireworks.Domain.Permissions;
using F.Fireworks.Infrastructure.Auth;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace F.Fireworks.Api.Features.Users;

public class CreateUserEndpoint(IMediator mediator) : Endpoint<CreateUserCommand>
{
    public override void Configure()
    {
        Post("users");
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        Policy(b => b.AddRequirements(new PermissionRequirement(PermissionDefinitions.UsersCreate)));
        Description(x => x.WithTags("Users"));
        Summary(s => s.Summary = "创建一个新用户");
    }

    public override async Task HandleAsync(CreateUserCommand req, CancellationToken ct)
    {
        var result = await mediator.Send(req, ct);
        await this.SendMyResultAsync(result, ct);
    }
}