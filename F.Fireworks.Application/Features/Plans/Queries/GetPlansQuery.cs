using Ardalis.Result;
using F.Fireworks.Application.DTOs.Common;
using F.Fireworks.Application.DTOs.Subscriptions;
using MediatR;

namespace F.Fireworks.Application.Features.Plans.Queries;

public record GetPlansQuery(PlanFilter Filter) : IRequest<Result<PaginatedList<PlanDto>>>;