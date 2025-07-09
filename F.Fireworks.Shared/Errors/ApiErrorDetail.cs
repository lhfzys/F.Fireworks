namespace F.Fireworks.Shared.Errors;

/// <summary>
///     用于在 ApiResponse 中表示单个验证错误的结构
/// </summary>
/// <param name="Field">发生错误的字段名 (camelCase)</param>
/// <param name="Message">具体的错误信息</param>
public record ApiErrorDetail(string Field, string Message);