using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Text;
using F.Fireworks.Application.Common.Attributes;
using F.Fireworks.Application.DTOs.Common;
using Microsoft.EntityFrameworkCore;

namespace F.Fireworks.Application.Common.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<T> ApplyFiltering<T>(this IQueryable<T> query, object filter)
    {
        var filterProperties = filter.GetType().GetProperties()
            .Where(p => p.GetValue(filter, null) != null).ToList();

        if (!filterProperties.Any())
            return query;

        var predicate = new StringBuilder();
        var parameters = new List<object>();
        var index = 0;

        foreach (var prop in filterProperties)
        {
            // 忽略分页和排序属性
            if (prop.Name is nameof(PagedAndSortedRequest.PageNumber) or nameof(PagedAndSortedRequest.PageSize) or
                nameof(PagedAndSortedRequest.SortField) or nameof(PagedAndSortedRequest.SortOrder) or
                "DateFrom" or "DateTo")
                continue;


            if (predicate.Length > 0)
                predicate.Append(" AND ");

            var op = prop.GetCustomAttribute<FilterOperatorAttribute>()?.Operator ??
                     GetDefaultOperator(prop.PropertyType);

            switch (op)
            {
                case FilterOperator.Contains:
                    // 对于字符串的 Contains，使用方法调用语法
                    predicate.Append($"{prop.Name}.Contains(@{index})");
                    break;
                case FilterOperator.GreaterThan:
                    predicate.Append($"{prop.Name} > @{index}");
                    break;
                case FilterOperator.LessThan:
                    predicate.Append($"{prop.Name} < @{index}");
                    break;
                case FilterOperator.GreaterThanOrEqual:
                    predicate.Append($"{prop.Name} >= @{index}");
                    break;
                case FilterOperator.LessThanOrEqual:
                    predicate.Append($"{prop.Name} <= @{index}");
                    break;
                case FilterOperator.Equals:
                default:
                    // 对于值类型或需要精确匹配的字符串，使用 "=="
                    predicate.Append($"{prop.Name} == @{index}");
                    break;
            }

            parameters.Add(prop.GetValue(filter)!);
            index++;
        }

        return predicate.Length > 0 ? query.Where(predicate.ToString(), parameters.ToArray()) : query;
    }


    // ... 私有帮助方法 ...
    private static FilterOperator GetDefaultOperator(Type type)
    {
        return type == typeof(string) ? FilterOperator.Contains : FilterOperator.Equals;
    }


    public static IQueryable<T> ApplySort<T>(this IQueryable<T> source, string? sortField, string? sortOrder)
    {
        if (string.IsNullOrWhiteSpace(sortField)) return source;

        var direction = sortOrder == "ascend" ? "" : " descending";
        return source.OrderBy($"{sortField}{direction}");
    }

    public static async Task<PaginatedList<T>> ToPaginatedListAsync<T>(
        this IQueryable<T> source, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        var total = await source.CountAsync(cancellationToken);
        var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);
        return new PaginatedList<T>(items, total, pageNumber, pageSize);
    }
}