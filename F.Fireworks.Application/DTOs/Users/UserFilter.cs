using F.Fireworks.Application.Common.Attributes;
using F.Fireworks.Application.DTOs.Common;
using F.Fireworks.Shared.Enums;

namespace F.Fireworks.Application.DTOs.Users;

public class UserFilter : PagedAndSortedRequest
{
    [FilterOperator(FilterOperator.Contains)]
    public string? UserName { get; set; }

    [FilterOperator(FilterOperator.Equals)]
    public UserStatus? Status { get; set; }

    [FilterOperator(FilterOperator.Equals)]
    public Guid? TenantId { get; set; }
}