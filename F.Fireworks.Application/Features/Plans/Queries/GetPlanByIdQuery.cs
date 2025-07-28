using Ardalis.Result;
using F.Fireworks.Application.DTOs.Subscriptions;
using MediatR;

namespace F.Fireworks.Application.Features.Plans.Queries;

public record GetPlanByIdQuery(Guid Id) : IRequest<Result<PlanDetailsDto>>;