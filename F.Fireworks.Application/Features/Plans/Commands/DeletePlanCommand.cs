using Ardalis.Result;
using MediatR;

namespace F.Fireworks.Application.Features.Plans.Commands;

public record DeletePlanCommand(Guid Id) : IRequest<Result>;