using Ardalis.Result;
using MediatR;

namespace F.Fireworks.Application.Features.Plans.Commands;

public record CreatePlanCommand(string Name, string? Description, List<Guid> PermissionIds) : IRequest<Result<Guid>>;