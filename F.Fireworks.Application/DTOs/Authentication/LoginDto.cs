namespace F.Fireworks.Application.DTOs.Authentication;

/// <summary>
///     登录成功后的响应，只包含 AccessToken
/// </summary>
/// <param name="AccessToken">访问令牌</param>
public record LoginResponse(string AccessToken);