using F.Fireworks.Api.Extensions;
using F.Fireworks.Application.Features.Library.Lessons.Commands;
using F.Fireworks.Domain.Permissions;
using F.Fireworks.Infrastructure.Auth;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace F.Fireworks.Api.Features.Library.Lessons;

public class DeleteLessonEndpoint(IMediator mediator) : Endpoint<DeleteLessonCommand, IResult>
{
    public override void Configure()
    {
        Delete("library/lessons/{Id}");
        Description(x => x.WithTags("Library.Lessons"));
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        Policy(b => b.AddRequirements(new PermissionRequirement(PermissionDefinitions.LibraryLessonsDelete)));
        Summary(s => s.Summary = "删除一个已存在的课节");
    }

    public override async Task HandleAsync(DeleteLessonCommand req, CancellationToken ct)
    {
        var result = await mediator.Send(req, ct);
        await SendAsync(result.ToMinimalApiResult(), cancellation: ct);
    }
}