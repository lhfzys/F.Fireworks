using F.Fireworks.Domain.Common;

namespace F.Fireworks.Domain.Courses;

/// <summary>
///     内容库 - 年级/级别 (如：小班、中班、大班)
/// </summary>
public class Grade : IEntity<Guid>, IAuditable
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public int SortOrder { get; set; }

    // --- 导航属性 ---
    public virtual ICollection<LibraryTopic> Topics { get; set; } = new List<LibraryTopic>();

    // --- 审计字段 ---
    public Guid? CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
    public Guid? LastModifiedBy { get; set; }
    public DateTime? LastModifiedOn { get; set; }
    public Guid Id { get; set; }
}