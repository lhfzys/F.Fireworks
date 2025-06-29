namespace F.Fireworks.Application.DTOs.Common;

public abstract class FilterBase
{
    public virtual int? PageNumber { get; set; } = 1;
    public virtual int? PageSize { get; set; } = 10;
    public virtual string? SortField { get; set; }
    public virtual string? SortOrder { get; set; } // "ascend" or "descend"
}