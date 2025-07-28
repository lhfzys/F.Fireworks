using Ardalis.Result;
using MediatR;

namespace F.Fireworks.Application.Features.Plans.Commands;

public record UpdatePlanCommand(
    Guid Id,
    string Name,
    string? Description,
    bool IsActive,
    List<Guid> PermissionIds) : IRequest<Result>;