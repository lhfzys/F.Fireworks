using Ardalis.Result;
using MediatR;

namespace F.Fireworks.Application.Features.Library.Grades.Commands;

public record CreateGradeCommand(string Name, string? Description, int SortOrder) : IRequest<Result<Guid>>;