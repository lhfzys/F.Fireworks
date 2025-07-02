using Ardalis.Result;
using MediatR;

namespace F.Fireworks.Application.Features.Roles.Commands;

public record DeleteRoleCommand(Guid Id) : IRequest<Result>;