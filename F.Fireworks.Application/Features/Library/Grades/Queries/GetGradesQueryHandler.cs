using Ardalis.Result;
using F.Fireworks.Application.Contracts.Persistence;
using F.Fireworks.Application.DTOs.Courses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace F.Fireworks.Application.Features.Library.Grades.Queries;

public class GetGradesQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetGradesQuery, Result<List<GradeDto>>>
{
    public async Task<Result<List<GradeDto>>> Handle(GetGradesQuery request, CancellationToken cancellationToken)
    {
        var grades = await context.Grades
            .AsNoTracking()
            .OrderBy(g => g.SortOrder)
            .Select(g => new GradeDto(g.Id, g.Name, g.Description, g.SortOrder))
            .ToListAsync(cancellationToken);
        return Result.Success(grades);
    }
}