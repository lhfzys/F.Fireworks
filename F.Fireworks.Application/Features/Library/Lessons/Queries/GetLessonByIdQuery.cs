using Ardalis.Result;
using F.Fireworks.Application.DTOs.Courses;
using MediatR;

namespace F.Fireworks.Application.Features.Library.Lessons.Queries;

public record GetLessonByIdQuery(Guid Id) : IRequest<Result<LibraryLessonDetailsDto>>;