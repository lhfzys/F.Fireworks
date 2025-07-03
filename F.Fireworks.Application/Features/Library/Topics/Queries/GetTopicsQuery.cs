using Ardalis.Result;
using F.Fireworks.Application.DTOs.Common;
using F.Fireworks.Application.DTOs.Courses;
using MediatR;

namespace F.Fireworks.Application.Features.Library.Topics.Queries;

public record GetTopicsQuery(TopicFilter Filter) : IRequest<Result<PaginatedList<LibraryTopicDto>>>;