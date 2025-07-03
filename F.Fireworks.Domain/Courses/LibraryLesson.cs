using F.Fireworks.Domain.Common;
using F.Fireworks.Shared.Enums;

namespace F.Fireworks.Domain.Courses;

/// <summary>
///     内容库 - 课节
/// </summary>
public class LibraryLesson : IEntity<Guid>, IAuditable
{
    public string Title { get; set; } = null!;
    public string? Content { get; set; }
    public string? VideoUrl { get; set; }
    public int DurationInMinutes { get; set; }
    public int SortOrder { get; set; }
    public bool IsTrial { get; set; }

    public LessonStatus Status { get; set; } = LessonStatus.Draft;

    // --- 外键和父级导航属性 ---
    public Guid LibraryTopicId { get; set; }
    public virtual LibraryTopic Topic { get; set; } = null!;

    // --- 审计字段 ---
    public Guid? CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
    public Guid? LastModifiedBy { get; set; }
    public DateTime? LastModifiedOn { get; set; }
    public Guid Id { get; set; }
}