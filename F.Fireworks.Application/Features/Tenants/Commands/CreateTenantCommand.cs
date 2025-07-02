using Ardalis.Result;
using F.Fireworks.Shared.Enums;
using MediatR;

namespace F.Fireworks.Application.Features.Tenants.Commands;

public record CreateTenantCommand(string Name, TenantType Type) : IRequest<Result<Guid>>;