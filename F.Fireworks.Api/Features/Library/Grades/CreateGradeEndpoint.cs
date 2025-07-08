using F.Fireworks.Api.Extensions;
using F.Fireworks.Application.Features.Library.Grades.Commands;
using F.Fireworks.Domain.Permissions;
using F.Fireworks.Infrastructure.Auth;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace F.Fireworks.Api.Features.Library.Grades;

public class CreateGradeEndpoint(IMediator mediator) : Endpoint<CreateGradeCommand>
{
    public override void Configure()
    {
        Post("library/grades");
        Description(x => x.WithTags("Library.Grades"));
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        Policy(b => b.AddRequirements(new PermissionRequirement(PermissionDefinitions.LibraryGradesCreate)));
        Summary(s => s.Summary = "创建一个新的年级");
    }

    public override async Task HandleAsync(CreateGradeCommand req, CancellationToken ct)
    {
        var result = await mediator.Send(req, ct);
        await this.SendMyResultAsync(result, ct);
    }
}