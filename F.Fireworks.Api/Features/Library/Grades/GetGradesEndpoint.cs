using F.Fireworks.Api.Extensions;
using F.Fireworks.Application.Features.Library.Grades.Queries;
using F.Fireworks.Domain.Permissions;
using F.Fireworks.Infrastructure.Auth;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace F.Fireworks.Api.Features.Library.Grades;

public class GetGradesEndpoint(IMediator mediator) : EndpointWithoutRequest<IResult>
{
    public override void Configure()
    {
        Get("library/grades");
        Description(x => x.WithTags("Library.Grades"));
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        Policy(b => b.AddRequirements(new PermissionRequirement(PermissionDefinitions.LibraryGradesRead)));
        Summary(s => s.Summary = "获取所有年级列表");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var result = await mediator.Send(new GetGradesQuery(), ct);
        await SendAsync(result.ToMinimalApiResult(), cancellation: ct);
    }
}