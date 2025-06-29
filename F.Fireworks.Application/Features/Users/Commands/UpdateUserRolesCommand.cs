using Ardalis.Result;
using MediatR;

namespace F.Fireworks.Application.Features.Users.Commands;

public record UpdateUserRolesCommand(Guid UserId, List<string> RoleNames) : IRequest<Result>;