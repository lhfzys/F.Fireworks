using F.Fireworks.Application.Common.Attributes;
using F.Fireworks.Application.DTOs.Common;
using F.Fireworks.Shared.Enums;

namespace F.Fireworks.Application.DTOs.Tenants;

public class TenantFilter : PagedAndSortedRequest
{
    [FilterOperator(FilterOperator.Contains)]
    public string? Name { get; set; }

    [FilterOperator(FilterOperator.Equals)]
    public TenantType? Type { get; set; }
}