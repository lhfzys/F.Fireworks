using Ardalis.Result;
using MediatR;

namespace F.Fireworks.Application.Features.Library.Grades.Commands;

public record DeleteGradeCommand(Guid Id) : IRequest<Result>;