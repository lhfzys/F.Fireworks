namespace F.Fireworks.Application.Contracts.Services;

public record AuditInfo(
    Guid? UserId,
    string? UserName,
    Guid TenantId,
    string? TenantName,
    string RequestName,
    string RequestUrl,
    string HttpMethod,
    int StatusCode,
    long ExecutionDurationMs,
    string IpAddress,
    string UserAgent,
    string? RequestData,
    string? ResponseData);

public interface IAuditService
{
    Task AuditAsync(AuditInfo auditInfo, CancellationToken cancellationToken = default);
}