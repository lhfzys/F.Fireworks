using Ardalis.Result;
using F.Fireworks.Application.DTOs.Tenants;
using MediatR;

namespace F.Fireworks.Application.Features.Tenants.Queries;

public record GetTenantByIdQuery(Guid Id) : IRequest<Result<TenantDetailsDto>>;