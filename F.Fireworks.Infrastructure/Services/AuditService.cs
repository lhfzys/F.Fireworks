using F.Fireworks.Application.Contracts.Persistence;
using F.Fireworks.Application.Contracts.Services;
using F.Fireworks.Domain.Logging;
using Hangfire;

namespace F.Fireworks.Infrastructure.Services;

public interface IAuditLogPersister
{
    Task PersistAsync(AuditLog log, IJobCancellationToken jobCancellationToken);
}

public class AuditLogPersister(IApplicationDbContext context) : IAuditLogPersister
{
    public async Task PersistAsync(AuditLog log, IJobCancellationToken jobCancellationToken)
    {
        jobCancellationToken.ThrowIfCancellationRequested();
        var cancellationToken = jobCancellationToken.ShutdownToken;
        await context.AuditLogs.AddAsync(log, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }
}

public class AuditService(IApplicationDbContext context, IBackgroundJobClient backgroundJobClient) : IAuditService
{
    public Task AuditAsync(AuditInfo auditInfo, CancellationToken cancellationToken = default)
    {
        var auditLog = new AuditLog
        {
            Id = Guid.NewGuid(),
            UserId = auditInfo.UserId,
            UserName = auditInfo.UserName,
            TenantId = auditInfo.TenantId,
            TenantName = auditInfo.TenantName,
            RequestName = auditInfo.RequestName,
            Url = auditInfo.RequestUrl,
            HttpMethod = auditInfo.HttpMethod,
            StatusCode = auditInfo.StatusCode,
            ExecutionDurationMs = auditInfo.ExecutionDurationMs,
            IpAddress = auditInfo.IpAddress,
            UserAgent = auditInfo.UserAgent,
            Timestamp = DateTime.UtcNow
        };
        backgroundJobClient.Enqueue<IAuditLogPersister>(persister =>
            persister.PersistAsync(auditLog, JobCancellationToken.Null));
        return Task.CompletedTask;
    }
}