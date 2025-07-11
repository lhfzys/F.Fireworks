using System.Text.Json.Serialization;
using F.Fireworks.Shared.Interfaces;

namespace F.Fireworks.Application.DTOs.Account;

public class MenuNodeDto : ITreeNode<MenuNodeDto, Guid>
{
    public string? DisplayName { get; set; }
    public string? Path { get; set; }
    public string? Icon { get; set; }
    public int SortOrder { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<MenuNodeDto> Children { get; set; } = [];

    public Guid Id { get; set; }

    [JsonIgnore] public Guid? ParentId { get; set; }
}