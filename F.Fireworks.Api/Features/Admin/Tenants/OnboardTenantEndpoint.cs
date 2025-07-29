using F.Fireworks.Api.Extensions;
using F.Fireworks.Application.Features.Plans.Commands;
using F.Fireworks.Domain.Permissions;
using F.Fireworks.Infrastructure.Auth;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace F.Fireworks.Api.Features.Admin.Tenants;

public class OnboardTenantEndpoint(IMediator mediator) : Endpoint<OnboardTenantCommand>
{
    public override void Configure()
    {
        Post("admin/onboard-tenant");
        Description(x => x.WithTags("Admin.Tenants"));
        Summary(s => s.Summary = "引导一个新租户（创建租户和其管理员）");
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        Policy(b => b.AddRequirements(new PermissionRequirement(PermissionDefinitions.TenantsCreate)));
    }

    public override async Task HandleAsync(OnboardTenantCommand req, CancellationToken ct)
    {
        var result = await mediator.Send(req, ct);
        await this.SendMyResultAsync(result, ct);
    }
}