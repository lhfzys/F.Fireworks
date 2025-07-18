using Ardalis.Result;
using F.Fireworks.Application.DTOs.Courses;
using MediatR;

namespace F.Fireworks.Application.Features.Library.Topics.Queries;

public record GetTopicByIdQuery(Guid Id) : IRequest<Result<LibraryTopicDto>>;