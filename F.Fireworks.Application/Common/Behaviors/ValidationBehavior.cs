using System.Reflection;
using Ardalis.Result;
using FluentValidation;
using MediatR;

namespace F.Fireworks.Application.Common.Behaviors;

public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : IResult
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!validators.Any()) return await next(cancellationToken);

        var context = new ValidationContext<TRequest>(request);

        var validationResults = await Task.WhenAll(
            validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        var failures = validationResults
            .SelectMany(r => r.Errors)
            .Where(f => f != null)
            .ToList();
        if (failures.Count == 0) return await next(cancellationToken);
        var errors = failures.Select(f => new ValidationError
        {
            Identifier = f.PropertyName,
            ErrorMessage = f.ErrorMessage,
            Severity = (ValidationSeverity)f.Severity // 可以转换严重级别
        }).ToList();

        var invalidMethod = typeof(TResponse).GetMethod(
            nameof(Result.Invalid),
            BindingFlags.Public | BindingFlags.Static, // 查找公共的静态方法
            [typeof(List<ValidationError>)]);
        if (invalidMethod is not null) return (TResponse)invalidMethod.Invoke(null, new object[] { errors });
        throw new InvalidOperationException(
            $"Could not find static method 'Invalid' on type '{typeof(TResponse).Name}'");
        // if (failures.Count != 0)
        // {
        //     var errors = failures.Select(f => new ValidationError
        //     {
        //         Identifier = f.PropertyName,
        //         ErrorMessage = f.ErrorMessage
        //     }).ToList();
        //
        //     // 使用反射创建失败的 Result<T> 实例
        //     var resultType = typeof(Result<>).MakeGenericType(typeof(TResponse).GetGenericArguments()[0]);
        //     var invalidMethod = resultType.GetMethod(nameof(Result.Invalid), [typeof(List<ValidationError>)]);
        //     return (TResponse)invalidMethod.Invoke(null, [errors]);
        // }

        // return await next(cancellationToken);
    }
}