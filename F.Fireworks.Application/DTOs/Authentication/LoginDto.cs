using Ardalis.Result;
using MediatR;

namespace F.Fireworks.Application.DTOs.Authentication;

public class LoginDto
{
    // 请求对象
    public record LoginCommand(string UserName, string Password) : IRequest<Result<LoginResponse>>;

// 响应对象
    public record LoginResponse(string AccessToken, string RefreshToken);
}