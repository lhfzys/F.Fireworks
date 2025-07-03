using F.Fireworks.Api.Extensions;
using F.Fireworks.Application.DTOs.Courses;
using F.Fireworks.Application.Features.Library.Lessons.Queries;
using F.Fireworks.Domain.Permissions;
using F.Fireworks.Infrastructure.Auth;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace F.Fireworks.Api.Features.Library.Lessons;

public class GetLessonsEndpoint(IMediator mediator) : Endpoint<LessonFilter, IResult>
{
    public override void Configure()
    {
        Get("library/lessons");
        Description(x => x.WithTags("Library.Lessons"));
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        Policy(b => b.AddRequirements(new PermissionRequirement(PermissionDefinitions.LibraryLessonsRead)));
        Summary(s => s.Summary = "获取所有小节列表");
    }

    public override async Task HandleAsync([AsParameters] LessonFilter request, CancellationToken ct)
    {
        var result = await mediator.Send(new GetLessonsQuery(request), ct);
        await SendAsync(result.ToMinimalApiResult(), cancellation: ct);
    }
}