using Ardalis.Result;
using F.Fireworks.Application.DTOs.Permissions;
using MediatR;

namespace F.Fireworks.Application.Features.Permissions.Queries;

public record GetAllPermissionsQuery : IRequest<Result<List<PermissionNodeDto>>>;