using F.Fireworks.Domain.Common;

namespace F.Fireworks.Domain.Logging;

public class AuditLog : IEntity<Guid>
{
    public Guid? UserId { get; set; }
    public string? UserName { get; set; }
    public Guid TenantId { get; set; }
    public string? TenantName { get; set; }
    public string HttpMethod { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string RequestName { get; set; } = string.Empty;
    public string IpAddress { get; set; } = string.Empty;
    public string UserAgent { get; set; } = string.Empty;
    public string? RequestData { get; set; }
    public string? ResponseData { get; set; }
    public int StatusCode { get; set; }
    public long ExecutionDurationMs { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public Guid Id { get; set; }
}