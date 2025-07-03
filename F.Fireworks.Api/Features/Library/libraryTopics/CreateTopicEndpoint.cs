using F.Fireworks.Api.Extensions;
using F.Fireworks.Application.Features.Library.Topics.Commands;
using F.Fireworks.Domain.Permissions;
using F.Fireworks.Infrastructure.Auth;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace F.Fireworks.Api.Features.Library.libraryTopics;

public class CreateTopicEndpoint(IMediator mediator) : Endpoint<CreateTopicCommand, IResult>
{
    public override void Configure()
    {
        Post("library/topics");
        Description(x => x.WithTags("Library.Topics"));
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        Policy(b => b.AddRequirements(new PermissionRequirement(PermissionDefinitions.LibraryTopicsCreate)));
        Summary(s => s.Summary = "创建一个新的专题");
    }

    public override async Task HandleAsync(CreateTopicCommand req, CancellationToken ct)
    {
        var result = await mediator.Send(req, ct);
        await SendAsync(result.ToMinimalApiResult(), cancellation: ct);
    }
}