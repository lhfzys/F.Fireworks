using F.Fireworks.Api.Extensions;
using F.Fireworks.Application.Features.Library.Lessons.Commands;
using F.Fireworks.Domain.Permissions;
using F.Fireworks.Infrastructure.Auth;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace F.Fireworks.Api.Features.Library.Lessons;

public class CreateLessonEndpoint(IMediator mediator) : Endpoint<CreateLessonCommand, IResult>
{
    public override void Configure()
    {
        Post("library/lessons");
        Description(x => x.WithTags("Library.Lessons"));
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        Policy(b => b.AddRequirements(new PermissionRequirement(PermissionDefinitions.LibraryLessonsCreate)));
        Summary(s => s.Summary = "创建一个新的课节");
    }

    public override async Task HandleAsync(CreateLessonCommand req, CancellationToken ct)
    {
        var result = await mediator.Send(req, ct);
        await SendAsync(result.ToMinimalApiResult(), cancellation: ct);
    }
}