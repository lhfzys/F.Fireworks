using System.Text.RegularExpressions;
using F.Fireworks.Application.Contracts.Services;

namespace F.Fireworks.Infrastructure.Services;

public class DataSanitizer : IDataSanitizer
{
    private static readonly string[] SensitiveKeywords =
    [
        "password", "token", "secret", "apiKey", "idCard"
    ];

    private static readonly Regex jsonRegex = new(
        $"\"({string.Join("|", SensitiveKeywords)})\"\\s*:\\s*\"(.*?)\"",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    public string Sanitize(string json)
    {
        return string.IsNullOrWhiteSpace(json)
            ? string.Empty
            : jsonRegex.Replace(json, match => $"\"{match.Groups[1].Value}\":\"[REDACTED]\"");
    }
}