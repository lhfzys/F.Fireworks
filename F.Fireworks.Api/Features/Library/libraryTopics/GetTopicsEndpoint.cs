using F.Fireworks.Api.Extensions;
using F.Fireworks.Application.DTOs.Courses;
using F.Fireworks.Application.Features.Library.Topics.Queries;
using F.Fireworks.Domain.Permissions;
using F.Fireworks.Infrastructure.Auth;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace F.Fireworks.Api.Features.Library.libraryTopics;

public class GetTopicsEndpoint(IMediator mediator) : Endpoint<TopicFilter>
{
    public override void Configure()
    {
        Get("library/topics");
        Description(x => x.WithTags("Library.Topics"));
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        Policy(b => b.AddRequirements(new PermissionRequirement(PermissionDefinitions.LibraryTopicsRead)));
        Summary(s => s.Summary = "获取所有专题列表");
    }

    public override async Task HandleAsync([AsParameters] TopicFilter request, CancellationToken ct)
    {
        var result = await mediator.Send(new GetTopicsQuery(request), ct);
        await this.SendMyResultAsync(result, ct);
    }
}