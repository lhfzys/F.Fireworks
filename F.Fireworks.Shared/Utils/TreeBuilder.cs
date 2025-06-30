using F.Fireworks.Shared.Interfaces;

namespace F.Fireworks.Shared.Utils;

public static class TreeBuilder
{
    /// <summary>
    ///     将一个扁平列表构建成树状结构
    /// </summary>
    /// <typeparam name="TNode">节点类型，必须实现 ITreeNode 接口</typeparam>
    /// <typeparam name="TKey">主键类型 (如 Guid, int)</typeparam>
    /// <param name="flatList">包含所有节点的扁平列表</param>
    /// <returns>只包含根节点的列表，子节点已挂载到对应的父节点下</returns>
    public static List<TNode> BuildTree<TNode, TKey>(IEnumerable<TNode> flatList)
        where TNode : ITreeNode<TNode, TKey>
        where TKey : struct, IEquatable<TKey>
    {
        var dictionary = flatList.ToDictionary(n => n.Id, n => n);
        var rootNodes = new List<TNode>();

        foreach (var node in dictionary.Values)
            if (node.ParentId.HasValue && dictionary.TryGetValue(node.ParentId.Value, out var parentNode))
                parentNode.Children.Add(node);
            else
                rootNodes.Add(node);

        return rootNodes;
    }
}