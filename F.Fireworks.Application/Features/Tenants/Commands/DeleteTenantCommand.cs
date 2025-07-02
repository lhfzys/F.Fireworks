using Ardalis.Result;
using MediatR;

namespace F.Fireworks.Application.Features.Tenants.Commands;

public record DeleteTenantCommand(Guid Id) : IRequest<Result>;