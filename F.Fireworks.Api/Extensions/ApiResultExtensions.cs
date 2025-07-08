using Ardalis.Result;
using F.Fireworks.Shared.Models;
using static Microsoft.AspNetCore.Http.StatusCodes;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace F.Fireworks.Api.Extensions;

public static class ApiResultExtensions
{
    public static Task WriteAsApiResponse<T>(this HttpContext httpContext, Result<T> result)
    {
        httpContext.Response.ContentType = "application/json; charset=utf-8";
        httpContext.Response.StatusCode = result.Status switch
        {
            ResultStatus.Ok => Status200OK,
            ResultStatus.Invalid => Status400BadRequest,
            ResultStatus.NotFound => Status404NotFound,
            ResultStatus.Unauthorized => Status401Unauthorized,
            ResultStatus.Forbidden => Status403Forbidden,
            _ => Status500InternalServerError
        };
        var apiResponse = result.ToApiResponse();
        return httpContext.Response.WriteAsJsonAsync(apiResponse, httpContext.RequestAborted);
    }

    private static ApiResponse<T> ToApiResponse<T>(this Result<T> result)
    {
        if (result.IsSuccess) return ApiResponse<T>.Success(result.Value, result.SuccessMessage);
        if (result.Status == ResultStatus.Invalid)
        {
            var invalidMessage = "Validation failed.";
            var errors = result.ValidationErrors.Select(e => e.ErrorMessage).ToList();
            return ApiResponse<T>.Fail(invalidMessage, errors);
        }

        return ApiResponse<T>.Fail(result.Errors.FirstOrDefault() ?? "An unexpected error occurred.");
    }

    public static IResult ToMinimalApiResult<T>(this Result<T> result)
    {
        if (result.IsSuccess) return Results.Ok(ApiResponse<T>.Success(result.Value));

        return result.Status switch
        {
            ResultStatus.NotFound => Results.NotFound(
                ApiResponse<T>.Fail(result.Errors.FirstOrDefault() ?? "Resource not found.")),
            ResultStatus.Invalid => Results.BadRequest(ApiResponse<T>.Fail("Validation failed.",
                result.ValidationErrors.Select(e => e.ErrorMessage).ToList())),
            ResultStatus.Unauthorized => Results.Unauthorized(),
            ResultStatus.Forbidden => Results.Forbid(),
            ResultStatus.Error => Results.Json(
                ApiResponse<T>.Fail(result.Errors.FirstOrDefault() ?? "An unexpected error occurred."),
                statusCode: Status500InternalServerError),
            _ => Results.Json(
                ApiResponse<T>.Fail("An unknown error occurred."),
                statusCode: Status500InternalServerError)
        };
    }

    public static IResult ToMinimalApiResult(this Result result)
    {
        if (result.IsSuccess) return Results.Ok(ApiResponse.Success());

        return result.Status switch
        {
            ResultStatus.NotFound => Results.NotFound(
                ApiResponse.Fail(result.Errors.FirstOrDefault() ?? "Resource not found.")),
            ResultStatus.Invalid => Results.BadRequest(ApiResponse.Fail("Validation failed.",
                result.ValidationErrors.Select(e => e.ErrorMessage).ToList())),
            ResultStatus.Unauthorized => Results.Unauthorized(),
            ResultStatus.Forbidden => Results.Forbid(),
            _ => Results.Json(ApiResponse.Fail(result.Errors.FirstOrDefault() ?? "An unexpected error occurred."),
                statusCode: Status500InternalServerError)
        };
    }
}