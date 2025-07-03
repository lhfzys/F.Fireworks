using Ardalis.Result;
using F.Fireworks.Application.Common.Extensions;
using F.Fireworks.Application.Contracts.Persistence;
using F.Fireworks.Application.DTOs.Common;
using F.Fireworks.Application.DTOs.Courses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace F.Fireworks.Application.Features.Library.Lessons.Queries;

public class GetLessonsQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetLessonsQuery, Result<PaginatedList<LibraryLessonDto>>>
{
    public async Task<Result<PaginatedList<LibraryLessonDto>>> Handle(GetLessonsQuery request,
        CancellationToken cancellationToken)
    {
        var query = context.LibraryLessons.Include(l => l.Topic).AsNoTracking();

        var paginatedResult = await query
            .ApplyFiltering(request.Filter)
            .Select(l => new LibraryLessonDto
            {
                Id = l.Id,
                Title = l.Title,
                SortOrder = l.SortOrder,
                IsTrial = l.IsTrial,
                Status = l.Status,
                DurationInMinutes = l.DurationInMinutes,
                LibraryTopicId = l.LibraryTopicId,
                TopicName = l.Topic.Name
            })
            .ApplySort(request.Filter.SortField, request.Filter.SortOrder)
            .ToPaginatedListAsync(request.Filter.PageNumber, request.Filter.PageSize, cancellationToken);
        return Result<PaginatedList<LibraryLessonDto>>.Success(paginatedResult);
    }
}