using Ardalis.Result;
using F.Fireworks.Application.Common.Extensions;
using FastEndpoints;

namespace F.Fireworks.Api.Extensions;

public static class EndpointExtensions
{
    /// <summary>
    ///     扩展方法：处理带返回数据的 Ardalis.Result
    /// </summary>
    public static Task SendMyResultAsync<T>(this IEndpoint endpoint, Result<T> result,
        CancellationToken cancellationToken = default)
    {
        var apiResponse = result.ToApiResponse();

        if (apiResponse.IsSuccess)
            return endpoint.HttpContext.Response.SendAsync(apiResponse, cancellation: cancellationToken);

        var statusCode = result.Status switch
        {
            ResultStatus.Invalid => 400,
            ResultStatus.NotFound => 404,
            ResultStatus.Unauthorized => 401,
            ResultStatus.Forbidden => 403,
            ResultStatus.Error => 500,
            _ => 500
        };

        return endpoint.HttpContext.Response.SendAsync(apiResponse, statusCode, cancellation: cancellationToken);
    }

    /// <summary>
    ///     扩展方法：处理不带返回数据的 Ardalis.Result
    /// </summary>
    public static Task SendMyResultAsync(this IEndpoint endpoint, Result result,
        CancellationToken cancellationToken = default)
    {
        var apiResponse = result.ToApiResponse();

        if (apiResponse.IsSuccess)
            return endpoint.HttpContext.Response.SendAsync(apiResponse, cancellation: cancellationToken);

        var statusCode = result.Status switch
        {
            ResultStatus.Invalid => 400,
            ResultStatus.NotFound => 404,
            ResultStatus.Unauthorized => 401,
            ResultStatus.Forbidden => 403,
            ResultStatus.Error => 500,
            _ => 500
        };

        return endpoint.HttpContext.Response.SendAsync(apiResponse, statusCode, cancellation: cancellationToken);
    }
}