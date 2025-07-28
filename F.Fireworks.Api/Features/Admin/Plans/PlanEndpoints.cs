using F.Fireworks.Api.Extensions;
using F.Fireworks.Application.DTOs.Subscriptions;
using F.Fireworks.Application.Features.Plans.Commands;
using F.Fireworks.Application.Features.Plans.Queries;
using F.Fireworks.Domain.Permissions;
using F.Fireworks.Infrastructure.Auth;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace F.Fireworks.Api.Features.Admin.Plans;

public sealed class PlanEndpointGroup : Group
{
    public PlanEndpointGroup()
    {
        Configure("admin/plans", ep =>
            {
                ep.Description(x => x.WithTags("Admin.Plans"));
                ep.AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
            }
        );
    }
}

public class GetPlansEndpoint(IMediator mediator) : Endpoint<PlanFilter>
{
    public override void Configure()
    {
        Get("/");
        Group<PlanEndpointGroup>();
        Policy(p => p.AddRequirements(new PermissionRequirement(PermissionDefinitions.PlansRead)));
        Summary(s => s.Summary = "获取套餐分页列表");
    }

    public override async Task HandleAsync([AsParameters] PlanFilter req, CancellationToken ct)
    {
        var query = new GetPlansQuery(req);
        var result = await mediator.Send(query, ct);
        await this.SendMyResultAsync(result, ct);
    }
}

public class GetPlanByIdEndpoint(IMediator mediator) : Endpoint<GetPlanByIdQuery>
{
    public override void Configure()
    {
        Get("/{Id:guid}");
        Group<PlanEndpointGroup>();
        Policy(p => p.AddRequirements(new PermissionRequirement(PermissionDefinitions.PlansRead)));
        Summary(s => s.Summary = "获取单个套餐详情");
    }

    public override async Task HandleAsync([AsParameters] GetPlanByIdQuery request, CancellationToken ct)
    {
        var result = await mediator.Send(request, ct);
        await this.SendMyResultAsync(result, ct);
    }
}

public class CreatePlanEndpoint(IMediator mediator) : Endpoint<CreatePlanCommand>
{
    public override void Configure()
    {
        Post("/");
        Group<PlanEndpointGroup>();
        Policy(b => b.AddRequirements(new PermissionRequirement(PermissionDefinitions.PlansCreate)));
        Summary(s => s.Summary = "创建一个新套餐");
    }

    public override async Task HandleAsync(CreatePlanCommand req, CancellationToken ct)
    {
        var result = await mediator.Send(req, ct);
        await this.SendMyResultAsync(result, ct);
    }
}

public class UpdatePlanEndpoint(IMediator mediator) : Endpoint<UpdatePlanCommand>
{
    public override void Configure()
    {
        Put("{Id:guid}");
        Group<PlanEndpointGroup>();
        Policy(b => b.AddRequirements(new PermissionRequirement(PermissionDefinitions.PlansUpdate)));
        Summary(s => s.Summary = "更新一个已存在的套餐");
    }

    public override async Task HandleAsync(UpdatePlanCommand req, CancellationToken ct)
    {
        var result = await mediator.Send(req, ct);
        await this.SendMyResultAsync(result, ct);
    }
}

public class DeletePlanEndpoint(IMediator mediator) : Endpoint<DeletePlanCommand>
{
    public override void Configure()
    {
        Delete("/{Id:guid}");
        Group<PlanEndpointGroup>();
        Policy(b => b.AddRequirements(new PermissionRequirement(PermissionDefinitions.PlansDelete)));
        Summary(s => s.Summary = "删除一个已存在的套餐");
    }

    public override async Task HandleAsync(DeletePlanCommand req, CancellationToken ct)
    {
        var result = await mediator.Send(req, ct);
        await this.SendMyResultAsync(result, ct);
    }
}