using Ardalis.Result;
using F.Fireworks.Application.DTOs.Courses;
using MediatR;

namespace F.Fireworks.Application.Features.Library.Grades.Queries;

public record GetGradesQuery : IRequest<Result<List<GradeDto>>>;