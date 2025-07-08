using Ardalis.Result;
using F.Fireworks.Shared.Models;

namespace F.Fireworks.Application.Common.Extensions;

public static class ResultExtensions
{
    public static ApiResponse<T> ToApiResponse<T>(this Result<T> result)
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

    public static ApiResponse ToApiResponse(this Result result)
    {
        if (result.IsSuccess) return ApiResponse.Success(result.SuccessMessage);

        if (result.Status == ResultStatus.Invalid)
        {
            var invalidMessage = "Validation failed.";
            var errors = result.ValidationErrors.Select(e => e.ErrorMessage).ToList();
            return ApiResponse.Fail(invalidMessage, errors);
        }

        return ApiResponse.Fail(result.Errors.FirstOrDefault() ?? "An unexpected error occurred.");
    }
}