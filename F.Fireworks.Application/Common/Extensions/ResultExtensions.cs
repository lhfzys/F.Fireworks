using Ardalis.Result;
using F.Fireworks.Shared.Errors;
using F.Fireworks.Shared.Models;

namespace F.Fireworks.Application.Common.Extensions;

public static class ResultExtensions
{
    public static ApiResponse<T> ToApiResponse<T>(this Result<T> result)
    {
        if (result.IsSuccess) return ApiResponse<T>.Success(result.Value, result.SuccessMessage);
        if (result.Status != ResultStatus.Invalid)
            return ApiResponse<T>.Fail(result.Errors.FirstOrDefault() ?? "An unexpected error occurred.");
        const string invalidMessage = "Validation failed.";
        var errors = result.ValidationErrors.Select(e => new ApiErrorDetail(
            char.ToLowerInvariant(e.Identifier[0]) + e.Identifier[1..],
            e.ErrorMessage
        )).ToList();
        return ApiResponse<T>.Fail(invalidMessage, errors);
    }

    public static ApiResponse ToApiResponse(this Result result)
    {
        if (result.IsSuccess) return ApiResponse.Success(result.SuccessMessage);

        if (result.Status != ResultStatus.Invalid)
            return ApiResponse.Fail(result.Errors.FirstOrDefault() ?? "An unexpected error occurred.");
        const string invalidMessage = "Validation failed.";
        var errors = result.ValidationErrors.Select(e => new ApiErrorDetail(
            char.ToLowerInvariant(e.Identifier[0]) + e.Identifier[1..],
            e.ErrorMessage
        )).ToList();
        return ApiResponse.Fail(invalidMessage, errors);
    }
}