using Ardalis.Result;
using F.Fireworks.Application.Contracts.Persistence;
using F.Fireworks.Application.DTOs.Courses;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace F.Fireworks.Application.Features.Library.Lessons.Queries;

public class GetLessonByIdQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetLessonByIdQuery, Result<LibraryLessonDetailsDto>>
{
    public async Task<Result<LibraryLessonDetailsDto>> Handle(GetLessonByIdQuery request,
        CancellationToken cancellationToken)
    {
        var lesson = await context.LibraryLessons
            .AsNoTracking()
            .Where(l => l.Id == request.Id)
            .ProjectToType<LibraryLessonDetailsDto>()
            .FirstOrDefaultAsync(cancellationToken);

        return lesson is not null ? Result.Success(lesson) : Result.NotFound();
    }
}