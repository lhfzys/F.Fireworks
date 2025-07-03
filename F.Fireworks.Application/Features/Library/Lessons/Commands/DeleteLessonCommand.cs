using Ardalis.Result;
using MediatR;

namespace F.Fireworks.Application.Features.Library.Lessons.Commands;

public record DeleteLessonCommand(Guid Id) : IRequest<Result>;