namespace F.Fireworks.Shared.Interfaces;

public interface ITreeNode<TNode, TKey> where TKey : struct
{
    TKey Id { get; }
    TKey? ParentId { get; }
    List<TNode> Children { get; set; }
}