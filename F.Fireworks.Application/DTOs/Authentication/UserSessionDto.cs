namespace F.Fireworks.Application.DTOs.Authentication;

public record UserSessionDto
{
    public Guid Id { get; init; }
    public string IpAddress { get; init; } = string.Empty;
    public string? Device { get; init; }
    public string? Location { get; init; }
    public DateTime CreatedOn { get; init; }
    public DateTime ExpiresOn { get; init; }
    public bool IsCurrent { get; set; }
}