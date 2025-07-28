using F.Fireworks.Application.Common.Attributes;
using F.Fireworks.Application.DTOs.Common;

namespace F.Fireworks.Application.DTOs.Subscriptions;

// 用于列表展示的DTO
public record PlanDto(Guid Id, string Name, string? Description, bool IsActive, List<Guid> PermissionIds);

// 用于详情展示和编辑回显的DTO
public record PlanDetailsDto(Guid Id, string Name, string? Description, bool IsActive, List<Guid> PermissionIds);

// 用于列表查询的Filter
public class PlanFilter : PagedAndSortedRequest
{
    [FilterOperator(FilterOperator.Contains)]
    public string? Name { get; set; }
}