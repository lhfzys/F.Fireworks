using System.Security.Cryptography;
using System.Text;

namespace F.Fireworks.Infrastructure.Common;

public static class DeterministicGuidGenerator
{
    /// <summary>
    ///     一个固定的命名空间，确保我们生成的GUID不会与其他地方的GUID冲突
    ///     这个GUID可以是任意一个有效的GUID
    /// </summary>
    private static readonly Guid Namespace = new("f8a2f1b9-3c7c-4c4f-8e2b-2d7a6b8a1f9a");

    public static Guid Create(string name)
    {
        if (string.IsNullOrEmpty(name))
            throw new ArgumentNullException(nameof(name));

        // 将命名空间和名称转换为字节数组
        var namespaceBytes = Namespace.ToByteArray();
        var nameBytes = Encoding.UTF8.GetBytes(name);

        // 合并字节数组
        var combinedBytes = new byte[namespaceBytes.Length + nameBytes.Length];
        Buffer.BlockCopy(namespaceBytes, 0, combinedBytes, 0, namespaceBytes.Length);
        Buffer.BlockCopy(nameBytes, 0, combinedBytes, namespaceBytes.Length, nameBytes.Length);

        // 使用 SHA1 哈希算法计算哈希值
        using var sha1 = SHA1.Create();
        var hash = sha1.ComputeHash(combinedBytes);

        // 从哈希值中提取16个字节来创建GUID
        var guidBytes = new byte[16];
        Array.Copy(hash, 0, guidBytes, 0, 16);

        // 根据RFC 4122, version 5, 设置版本号和变体
        guidBytes[6] = (byte)((guidBytes[6] & 0x0F) | (5 << 4)); // Version 5
        guidBytes[8] = (byte)((guidBytes[8] & 0x3F) | 0x80); // Variant

        // 字节序调整，以匹配.NET的Guid结构
        SwapByteOrder(guidBytes);

        return new Guid(guidBytes);
    }

    // .NET Guid 的字节序与标准RFC 4122不同，需要进行调整
    private static void SwapByteOrder(byte[] guid)
    {
        Swap(guid, 0, 3);
        Swap(guid, 1, 2);
        Swap(guid, 4, 5);
        Swap(guid, 6, 7);
    }

    private static void Swap(byte[] array, int i, int j)
    {
        (array[i], array[j]) = (array[j], array[i]);
    }
}