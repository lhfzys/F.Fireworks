using Ardalis.Result;
using F.Fireworks.Application.Contracts.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace F.Fireworks.Application.Features.Library.Grades.Commands;

public class UpdateGradeCommandHandler(IApplicationDbContext context) : IRequestHandler<UpdateGradeCommand, Result>
{
    public async Task<Result> Handle(UpdateGradeCommand request, CancellationToken cancellationToken)
    {
        var grade = await context.Grades
            .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);
        if (grade == null)
            return Result.NotFound("年级/级别不存在");
        grade.Name = request.Name;
        grade.SortOrder = request.SortOrder;
        grade.Description = request.Description;
        await context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}