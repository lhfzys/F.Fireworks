namespace F.Fireworks.Application.DTOs.Auditing;

public record AuditLogDto(
    Guid Id,
    DateTime Timestamp,
    Guid? UserId,
    string? UserName,
    Guid TenantId,
    string? TenantName,
    string RequestName,
    string HttpMethod,
    string Url,
    int StatusCode,
    long ExecutionDurationMs,
    string IpAddress);