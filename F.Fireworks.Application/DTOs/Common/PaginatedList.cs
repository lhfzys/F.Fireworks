namespace F.Fireworks.Application.DTOs.Common;

/// <summary>
///     通用的分页响应结果
/// </summary>
public class PaginatedList<T>
{
    public PaginatedList(IReadOnlyList<T> items, int total, int pageNumber, int pageSize)
    {
        Items = items;
        TotalCount = total;
        PageNumber = pageNumber;
        PageSize = pageSize;
    }

    public IReadOnlyList<T> Items { get; }
    public int TotalCount { get; }
    public int PageSize { get; }
    public int PageNumber { get; }

    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;
}