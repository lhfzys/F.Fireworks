using Ardalis.Result;
using MediatR;

namespace F.Fireworks.Application.Features.Authentication.Commands;

/// <summary>
///     吊销一个指定会话（RefreshToken）的命令
/// </summary>
/// <param name="SessionId">要吊销的会话ID (即 RefreshToken 的主键ID)</param>
public record RevokeSessionCommand(Guid SessionId) : IRequest<Result>;