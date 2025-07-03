using F.Fireworks.Api.Extensions;
using F.Fireworks.Application.Features.Library.Lessons.Queries;
using F.Fireworks.Domain.Permissions;
using F.Fireworks.Infrastructure.Auth;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace F.Fireworks.Api.Features.Library.Lessons;

public class GetLessonByIdEndpoint(IMediator mediator) : Endpoint<GetLessonByIdQuery, IResult>
{
    public override void Configure()
    {
        Get("library/lessons/{Id}");
        Description(x => x.WithTags("Library.Lessons"));
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        Policy(b => b.AddRequirements(new PermissionRequirement(PermissionDefinitions.LibraryLessonsRead)));
        Summary(s => s.Summary = "获取所有课节详情");
    }

    public override async Task HandleAsync([AsParameters] GetLessonByIdQuery request, CancellationToken ct)
    {
        var result = await mediator.Send(request, ct);
        await SendAsync(result.ToMinimalApiResult(), cancellation: ct);
    }
}