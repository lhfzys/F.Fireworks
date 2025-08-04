using Ardalis.Result;
using F.Fireworks.Application.DTOs.Common;
using F.Fireworks.Application.DTOs.Roles;
using MediatR;

namespace F.Fireworks.Application.Features.Roles.Queries;

public record GetAllRolesQuery(RoleFilter Filter) : IRequest<Result<PaginatedList<RoleDto>>>;