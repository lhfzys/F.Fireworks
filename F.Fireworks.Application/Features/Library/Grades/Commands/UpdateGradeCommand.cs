using Ardalis.Result;
using MediatR;

namespace F.Fireworks.Application.Features.Library.Grades.Commands;

public record UpdateGradeCommand(Guid Id, string Name, string? Description, int SortOrder) : IRequest<Result>;