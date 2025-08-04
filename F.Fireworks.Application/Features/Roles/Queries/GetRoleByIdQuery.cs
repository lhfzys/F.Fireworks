using Ardalis.Result;
using F.Fireworks.Application.DTOs.Roles;
using MediatR;

namespace F.Fireworks.Application.Features.Roles.Queries;

public record GetRoleByIdQuery(Guid Id) : IRequest<Result<RoleDetailsDto>>;