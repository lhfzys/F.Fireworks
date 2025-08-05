using Ardalis.Result;
using F.Fireworks.Shared.Enums;
using MediatR;

namespace F.Fireworks.Application.Features.Users.Commands;

public record UpdateUserCommand(
    Guid Id,
    string UserName,
    string Email,
    UserStatus Status) : IRequest<Result>;