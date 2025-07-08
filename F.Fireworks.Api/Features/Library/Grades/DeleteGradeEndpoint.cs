using F.Fireworks.Api.Extensions;
using F.Fireworks.Application.Features.Library.Grades.Commands;
using F.Fireworks.Domain.Permissions;
using F.Fireworks.Infrastructure.Auth;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace F.Fireworks.Api.Features.Library.Grades;

public class DeleteGradeEndpoint(IMediator mediator) : Endpoint<DeleteGradeCommand>
{
    public override void Configure()
    {
        Delete("library/grades/{Id}");
        Description(x => x.WithTags("Library.Grades"));
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        Policy(b => b.AddRequirements(new PermissionRequirement(PermissionDefinitions.LibraryGradesDelete)));
        Summary(s => s.Summary = "删除一个已存在的年级/级别");
    }

    public override async Task HandleAsync(DeleteGradeCommand req, CancellationToken ct)
    {
        var result = await mediator.Send(req, ct);
        await this.SendMyResultAsync(result, ct);
    }
}