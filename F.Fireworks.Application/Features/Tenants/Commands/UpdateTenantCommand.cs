using Ardalis.Result;
using MediatR;

namespace F.Fireworks.Application.Features.Tenants.Commands;

public record UpdateTenantCommand(Guid Id, string Name, bool IsActive) : IRequest<Result>;