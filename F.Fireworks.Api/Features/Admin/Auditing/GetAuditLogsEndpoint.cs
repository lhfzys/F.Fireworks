using F.Fireworks.Api.Extensions;
using F.Fireworks.Application.DTOs.Auditing;
using F.Fireworks.Application.Features.Auditing.Queries;
using F.Fireworks.Domain.Permissions;
using F.Fireworks.Infrastructure.Auth;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace F.Fireworks.Api.Features.Admin.Auditing;

public class GetAuditLogsEndpoint(IMediator mediator) : Endpoint<AuditLogFilter>
{
    public override void Configure()
    {
        Get("admin/audit-logs");
        Description(x => x.WithTags("Admin.AuditLogs"));
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        Policy(b => b.AddRequirements(new PermissionRequirement(PermissionDefinitions.AuditLogsRead)));
        Summary(s => s.Summary = "获取审计日志分页列表");
    }

    public override async Task HandleAsync([AsParameters] AuditLogFilter req, CancellationToken ct)
    {
        var query = new GetAuditLogsQuery(req);
        var result = await mediator.Send(query, ct);
        await this.SendMyResultAsync(result, ct);
    }
}