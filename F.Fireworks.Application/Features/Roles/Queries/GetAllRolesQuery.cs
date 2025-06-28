using Ardalis.Result;
using F.Fireworks.Application.DTOs.Roles;
using MediatR;

namespace F.Fireworks.Application.Features.Roles.Queries;

public record GetAllRolesQuery : IRequest<Result<List<RoleDto>>>;