using F.Fireworks.Api.Extensions;
using F.Fireworks.Application.Features.Library.Topics.Queries;
using F.Fireworks.Domain.Permissions;
using F.Fireworks.Infrastructure.Auth;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace F.Fireworks.Api.Features.Library.libraryTopics;

public class GetTopicByIdEndpoint(IMediator mediator) : Endpoint<GetTopicByIdQuery>
{
    public override void Configure()
    {
        Get("library/topics/{Id}");
        Description(x => x.WithTags("Library.Topics"));
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        Policy(b => b.AddRequirements(new PermissionRequirement(PermissionDefinitions.LibraryTopicsRead)));
        Summary(s => s.Summary = "获取专题详情");
    }

    public override async Task HandleAsync([AsParameters] GetTopicByIdQuery request, CancellationToken ct)
    {
        var result = await mediator.Send(request, ct);
        await this.SendMyResultAsync(result, ct);
    }
}