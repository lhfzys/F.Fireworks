using System.ComponentModel.DataAnnotations;

namespace F.Fireworks.Infrastructure.Options;

public class JwtSettings
{
    public const string SectionName = "JwtSettings";

    [Required] public string Issuer { get; init; } = null!;
    [Required] public string Audience { get; init; } = null!;
    [Required] public string SecretKey { get; init; } = null!;
    [Required] public double ExpirationDays { get; init; }
    [Required] public double DurationInMinutes { get; init; }
}