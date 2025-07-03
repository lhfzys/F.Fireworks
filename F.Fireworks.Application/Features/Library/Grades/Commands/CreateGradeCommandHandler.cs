using Ardalis.Result;
using F.Fireworks.Application.Contracts.Persistence;
using F.Fireworks.Domain.Courses;
using MediatR;

namespace F.Fireworks.Application.Features.Library.Grades.Commands;

public class CreateGradeCommandHandler(IApplicationDbContext context)
    : IRequestHandler<CreateGradeCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateGradeCommand request, CancellationToken cancellationToken)
    {
        var grade = new Grade
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            SortOrder = request.SortOrder
        };

        await context.Grades.AddAsync(grade, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success(grade.Id);
    }
}