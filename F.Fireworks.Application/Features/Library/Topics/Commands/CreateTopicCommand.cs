using Ardalis.Result;
using MediatR;

namespace F.Fireworks.Application.Features.Library.Topics.Commands;

public record CreateTopicCommand(
    string Name,
    Guid GradeId,
    int SortOrder,
    string? Description,
    string? CoverImageUrl) : IRequest<Result<Guid>>;