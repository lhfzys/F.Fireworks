using Ardalis.Result;
using F.Fireworks.Application.DTOs.Common;
using F.Fireworks.Application.DTOs.Tenants;
using MediatR;

namespace F.Fireworks.Application.Features.Tenants.Queries;

public record GetTenantsQuery(TenantFilter Filter) : IRequest<Result<PaginatedList<TenantDto>>>;