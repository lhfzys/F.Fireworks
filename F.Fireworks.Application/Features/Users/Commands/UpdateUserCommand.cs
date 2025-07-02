using Ardalis.Result;
using F.Fireworks.Shared.Enums;
using MediatR;

namespace F.Fireworks.Application.Features.Users.Commands;

public record UpdateUserCommand(
    Guid Id,
    UserStatus Status) : IRequest<Result>;