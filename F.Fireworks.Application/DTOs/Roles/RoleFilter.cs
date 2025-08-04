using F.Fireworks.Application.Common.Attributes;
using F.Fireworks.Application.DTOs.Common;

namespace F.Fireworks.Application.DTOs.Roles;

public class RoleFilter : PagedAndSortedRequest
{
    [FilterOperator(FilterOperator.Contains)]
    public string? Name { get; set; }

    [FilterOperator(FilterOperator.Equals)]
    public Guid? TenantId { get; set; }
}