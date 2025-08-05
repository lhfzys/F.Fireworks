using Ardalis.Result;
using MediatR;

namespace F.Fireworks.Application.Features.Roles.Commands;

public record UpdateRoleCommand(
    Guid Id,
    string Name,
    string? Description) : IRequest<Result>;