using Ardalis.Result;
using MediatR;

namespace F.Fireworks.Application.Features.Roles.Commands;

public record CreateRoleCommand(string Name, string? Description) : IRequest<Result<Guid>>;