using Ardalis.Result;
using MediatR;

namespace F.Fireworks.Application.Features.Users.Commands;

public record DeleteUserCommand(Guid Id) : IRequest<Result>;