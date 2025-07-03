using F.Fireworks.Application.DTOs.Common;

namespace F.Fireworks.Application.DTOs.Courses;

public class LessonFilter : PagedAndSortedRequest
{
    public string? Title { get; set; }
    public Guid? LibraryTopicId { get; set; }
}