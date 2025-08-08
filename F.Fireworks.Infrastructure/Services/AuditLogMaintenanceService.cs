using F.Fireworks.Application.Contracts.Persistence;
using F.Fireworks.Application.Contracts.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace F.Fireworks.Infrastructure.Services;

public class AuditLogMaintenanceService(IApplicationDbContext context, ILogger<AuditLogMaintenanceService> logger)
    : IAuditLogMaintenanceService
{
    public async Task PurgeOldLogsAsync(DateTime cutoffDate, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Starting to purge audit logs older than {CutoffDate}", cutoffDate);
        var rowsAffected = await context.AuditLogs
            .Where(log => log.Timestamp < cutoffDate)
            .ExecuteDeleteAsync(cancellationToken);
        logger.LogInformation("Successfully purged {Count} old audit logs.", rowsAffected);
    }
}