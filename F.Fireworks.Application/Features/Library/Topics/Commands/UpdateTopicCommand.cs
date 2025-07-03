using Ardalis.Result;
using MediatR;

namespace F.Fireworks.Application.Features.Library.Topics.Commands;

public record UpdateTopicCommand(
    Guid Id,
    string Name,
    int SortOrder,
    string? Description,
    string? CoverImageUrl) : IRequest<Result<Guid>>;