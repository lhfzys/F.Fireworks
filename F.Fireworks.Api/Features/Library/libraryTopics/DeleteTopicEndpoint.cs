using F.Fireworks.Api.Extensions;
using F.Fireworks.Application.Features.Library.Topics.Commands;
using F.Fireworks.Domain.Permissions;
using F.Fireworks.Infrastructure.Auth;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace F.Fireworks.Api.Features.Library.Grades;

public class DeleteTopicEndpoint(IMediator mediator) : Endpoint<DeleteTopicCommand>
{
    public override void Configure()
    {
        Delete("library/topics/{Id}");
        Description(x => x.WithTags("Library.Topics"));
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        Policy(b => b.AddRequirements(new PermissionRequirement(PermissionDefinitions.LibraryTopicsDelete)));
        Summary(s => s.Summary = "删除一个已存在的专题");
    }

    public override async Task HandleAsync(DeleteTopicCommand req, CancellationToken ct)
    {
        var result = await mediator.Send(req, ct);
        await this.SendMyResultAsync(result, ct);
    }
}