using Ardalis.Result;
using F.Fireworks.Application.DTOs.Authentication;
using MediatR;

namespace F.Fireworks.Application.Features.Authentication.Commands;

/// <summary>
///     登录命令
/// </summary>
/// <param name="TenantIdentifier">租户标识 (例如租户的唯一名称)</param>
/// <param name="Identifier">用户名 或 邮箱</param>
/// <param name="Password">密码</param>
public record LoginCommand(string TenantIdentifier, string Identifier, string Password)
    : IRequest<Result<LoginResponse>>;