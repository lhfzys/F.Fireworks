using Ardalis.Result;
using MediatR;

namespace F.Fireworks.Application.Features.Tenants.Commands;

public record PurgeTenantCommand(Guid TenantId) : IRequest<Result>;