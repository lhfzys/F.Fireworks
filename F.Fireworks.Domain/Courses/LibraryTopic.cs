using F.Fireworks.Domain.Common;

namespace F.Fireworks.Domain.Courses;

/// <summary>
///     内容库 - 专题/单元
/// </summary>
public class LibraryTopic : IEntity<Guid>, IAuditable
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string? CoverImageUrl { get; set; }
    public int SortOrder { get; set; }

    // --- 外键和父级导航属性 ---
    public Guid GradeId { get; set; }
    public virtual Grade Grade { get; set; } = null!;

    // --- 子级导航属性 ---
    public virtual ICollection<LibraryLesson> Lessons { get; set; } = new List<LibraryLesson>();
    public Guid? CreatedBy { get; set; }

    // --- 审计字段 ---
    public DateTime CreatedOn { get; set; }
    public Guid? LastModifiedBy { get; set; }
    public DateTime? LastModifiedOn { get; set; }
    public Guid Id { get; set; }
}