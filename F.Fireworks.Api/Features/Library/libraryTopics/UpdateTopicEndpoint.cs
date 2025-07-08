using F.Fireworks.Api.Extensions;
using F.Fireworks.Application.Features.Library.Topics.Commands;
using F.Fireworks.Domain.Permissions;
using F.Fireworks.Infrastructure.Auth;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace F.Fireworks.Api.Features.Library.Grades;

public class UpdateTopicEndpoint(IMediator mediator) : Endpoint<UpdateTopicCommand>
{
    public override void Configure()
    {
        Put("library/topics/{Id}");
        Description(x => x.WithTags("Library.Topics"));
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        Policy(b => b.AddRequirements(new PermissionRequirement(PermissionDefinitions.LibraryTopicsUpdate)));
        Summary(s => s.Summary = "更新一个已存在的专题");
    }

    public override async Task HandleAsync(UpdateTopicCommand req, CancellationToken ct)
    {
        var result = await mediator.Send(req, ct);
        await this.SendMyResultAsync(result, ct);
    }
}