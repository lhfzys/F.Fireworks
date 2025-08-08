using F.Fireworks.Application.Contracts.Services;
using Microsoft.Extensions.Logging;

namespace F.Fireworks.Infrastructure.BackgroundJobs;

public class AuditLogCleanupJob(IAuditLogMaintenanceService maintenanceService, ILogger<AuditLogCleanupJob> logger)
{
    public async Task Run()
    {
        logger.LogInformation("AuditLogCleanupJob is running.");

        var cutoffDate = DateTime.UtcNow.AddDays(-10);

        await maintenanceService.PurgeOldLogsAsync(cutoffDate);

        logger.LogInformation("AuditLogCleanupJob has completed.");
    }
}