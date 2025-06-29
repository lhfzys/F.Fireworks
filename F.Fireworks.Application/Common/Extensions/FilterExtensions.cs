using System.Linq.Expressions;
using System.Reflection;

namespace F.Fireworks.Application.Common.Extensions;

[AttributeUsage(AttributeTargets.Property)]
public sealed class QueryFilterAttribute : Attribute
{
    public QueryFilterAttribute(string? targetProperty = null, bool exactMatch = false)
    {
        TargetProperty = targetProperty;
        ExactMatch = exactMatch;
    }

    public string? TargetProperty { get; }
    public bool ExactMatch { get; }
}

public static class FilterExtensions
{
    public static IQueryable<TEntity> ApplyFilterFrom<TEntity, TFilter>(
        this IQueryable<TEntity> query, TFilter? filter)
    {
        if (filter is null) return query;

        var entityType = typeof(TEntity);
        var filterType = typeof(TFilter);
        var parameter = Expression.Parameter(entityType, "e");
        Expression? all = null;

        foreach (var prop in filterType.GetProperties())
        {
            var attr = prop.GetCustomAttribute<QueryFilterAttribute>();
            if (attr is null) continue;

            var value = prop.GetValue(filter);
            if (value is null || (value is string s && string.IsNullOrWhiteSpace(s))) continue;

            var entityPropName = attr.TargetProperty ?? prop.Name;
            var entityProp = entityType.GetProperty(entityPropName);
            if (entityProp is null) continue;

            var left = Expression.Property(parameter, entityProp);
            var right = Expression.Constant(value);
            Expression body;

            if (entityProp.PropertyType == typeof(string) && !attr.ExactMatch)
                body = Expression.Call(left, nameof(string.Contains), Type.EmptyTypes, right);
            else
                body = Expression.Equal(left, right);

            all = all is null ? body : Expression.AndAlso(all, body);
        }

        return all is null ? query : query.Where(Expression.Lambda<Func<TEntity, bool>>(all, parameter));
    }
}