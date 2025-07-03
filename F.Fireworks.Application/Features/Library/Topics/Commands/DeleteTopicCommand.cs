using Ardalis.Result;
using MediatR;

namespace F.Fireworks.Application.Features.Library.Topics.Commands;

public record DeleteTopicCommand(Guid Id) : IRequest<Result>;