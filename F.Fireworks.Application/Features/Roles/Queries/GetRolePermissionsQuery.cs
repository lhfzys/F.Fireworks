using Ardalis.Result;
using MediatR;

namespace F.Fireworks.Application.Features.Roles.Queries;

public record GetRolePermissionsQuery(Guid RoleId) : IRequest<Result<List<Guid>>>;