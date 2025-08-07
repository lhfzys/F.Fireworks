using F.Fireworks.Application.Common.Attributes;
using F.Fireworks.Application.DTOs.Common;

namespace F.Fireworks.Application.DTOs.Auditing;

public class AuditLogFilter : PagedAndSortedRequest
{
    [FilterOperator(FilterOperator.Contains)]
    public string? UserName { get; set; }

    // 允许按请求名称进行模糊查询 (通常是 Command/Query 的名称)
    [FilterOperator(FilterOperator.Contains)]
    public string? RequestName { get; set; }

    // 日期范围筛选（我们将在Handler中手动处理）
    public DateTime? DateFrom { get; set; }
    public DateTime? DateTo { get; set; }

    // 仅供超级管理员使用
    [FilterOperator(FilterOperator.Equals)]
    public Guid? TenantId { get; set; }
}