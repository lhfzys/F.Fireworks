using Ardalis.Result;
using F.Fireworks.Shared.Models;
using static Microsoft.AspNetCore.Http.StatusCodes;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace F.Fireworks.Api.Extensions;

public static class ApiResultExtensions
{
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