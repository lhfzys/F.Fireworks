namespace F.Fireworks.Application.Common.Attributes;

public enum FilterOperator
{
    Equals,
    Contains,
    GreaterThan,
    LessThan,
    GreaterThanOrEqual,
    LessThanOrEqual
}

[AttributeUsage(AttributeTargets.Property)]
public class FilterOperatorAttribute(FilterOperator op) : Attribute
{
    public FilterOperator Operator { get; } = op;
}