using Ardalis.Result;
using MediatR;

namespace F.Fireworks.Application.Features.Roles.Commands;

public record UpdateRolePermissionsCommand(Guid RoleId, List<Guid> PermissionIds) : IRequest<Result>;