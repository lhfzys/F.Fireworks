namespace F.Fireworks.Application.DTOs.Common;

public abstract class PagedAndSortedRequest
{
    /// <summary>
    ///     页码 (从 1 开始)
    /// </summary>
    public int PageNumber { get; set; } = 1;

    /// <summary>
    ///     每页数量
    /// </summary>
    public int PageSize { get; set; } = 10;

    /// <summary>
    ///     排序字段
    /// </summary>
    public string? SortField { get; set; }

    /// <summary>
    ///     排序顺序 ("ascend" 或 "descend")
    /// </summary>
    public string? SortOrder { get; set; }
}