using F.Fireworks.Api.Extensions;
using F.Fireworks.Application.Features.Library.Grades.Commands;
using F.Fireworks.Domain.Permissions;
using F.Fireworks.Infrastructure.Auth;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace F.Fireworks.Api.Features.Library.Grades;

public class UpdateGradeEndpoint(IMediator mediator) : Endpoint<UpdateGradeCommand, IResult>
{
    public override void Configure()
    {
        Put("library/grades/{Id}");
        Description(x => x.WithTags("Library.Grades"));
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        Policy(b => b.AddRequirements(new PermissionRequirement(PermissionDefinitions.LibraryGradesUpdate)));
        Summary(s => s.Summary = "更新一个已存在的年级/级别");
    }

    public override async Task HandleAsync(UpdateGradeCommand req, CancellationToken ct)
    {
        var result = await mediator.Send(req, ct);
        await SendAsync(result.ToMinimalApiResult(), cancellation: ct);
    }
}