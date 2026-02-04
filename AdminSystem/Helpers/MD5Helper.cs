using System.Security.Cryptography;
using System.Text;

namespace AdminSystem.Helpers;

/// <summary>
/// MD5 加密帮助类
/// </summary>
public static class MD5Helper
{
    /// <summary>
    /// MD5 加密（32位大写）
    /// </summary>
    /// <param name="input">待加密字符串</param>
    /// <returns>加密后的字符串</returns>
    public static string Encrypt(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return string.Empty;
        }

        using var md5 = MD5.Create();
        var inputBytes = Encoding.UTF8.GetBytes(input);
        var hashBytes = md5.ComputeHash(inputBytes);

        return Convert.ToHexString(hashBytes).ToUpper(); // 确保大写
    }

    /// <summary>
    /// 验证 MD5 加密
    /// </summary>
    /// <param name="input">待验证字符串</param>
    /// <param name="hash">MD5 哈希值</param>
    /// <returns>是否匹配</returns>
    public static bool Verify(string input, string hash)
    {
        var inputHash = Encrypt(input);
        return string.Equals(inputHash, hash, StringComparison.OrdinalIgnoreCase);
    }
}
