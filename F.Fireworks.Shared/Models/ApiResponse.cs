using F.Fireworks.Shared.Errors;

namespace F.Fireworks.Shared.Models;

public class ApiResponse<T>
{
    public bool IsSuccess { get; set; }
    public T? Data { get; set; }
    public string? Message { get; set; }

    public List<ApiErrorDetail>? Errors { get; set; }

    public static ApiResponse<T> Success(T data, string? message = null)
    {
        return new ApiResponse<T> { IsSuccess = true, Data = data, Message = message };
    }

    public static ApiResponse<T> Fail(string message, List<ApiErrorDetail>? errors = null)
    {
        return new ApiResponse<T> { IsSuccess = false, Message = message, Errors = errors };
    }
}

/// <summary>
///     统一的 API 响应格式 (非泛型版本，用于无返回数据的场景，如登出)
/// </summary>
public class ApiResponse : ApiResponse<object>
{
    public static ApiResponse Success(string? message = "Operation successful.")
    {
        return new ApiResponse { IsSuccess = true, Data = null, Message = message };
    }

    public new static ApiResponse Fail(string message, List<ApiErrorDetail>? errors = null)
    {
        return new ApiResponse { IsSuccess = false, Data = null, Message = message, Errors = errors };
    }
}