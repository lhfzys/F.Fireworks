using Ardalis.Result;
using F.Fireworks.Application.Contracts.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace F.Fireworks.Application.Features.Library.Grades.Commands;

public class DeleteGradeCommandHandler(IApplicationDbContext context) : IRequestHandler<DeleteGradeCommand, Result>
{
    public async Task<Result> Handle(DeleteGradeCommand request, CancellationToken cancellationToken)
    {
        var grade = await context.Grades
            .Include(g => g.Topics)
            .FirstOrDefaultAsync(g => g.Id == request.Id, cancellationToken);

        if (grade == null)
            return Result.NotFound("年级/级别不存在");

        if (grade.Topics.Count != 0) return Result.Error("该年级下有多个课程关联，禁止删除");

        context.Grades.Remove(grade);
        await context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}