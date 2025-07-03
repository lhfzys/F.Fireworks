using Ardalis.Result;
using F.Fireworks.Application.DTOs.Common;
using F.Fireworks.Application.DTOs.Courses;
using MediatR;

namespace F.Fireworks.Application.Features.Library.Lessons.Queries;

public record GetLessonsQuery(LessonFilter Filter) : IRequest<Result<PaginatedList<LibraryLessonDto>>>;