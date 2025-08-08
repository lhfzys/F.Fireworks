namespace F.Fireworks.Application.Contracts.Services;

public interface IAuditLogMaintenanceService
{
    /// <summary>
    ///     清除早于指定日期的审计日志
    /// </summary>
    Task PurgeOldLogsAsync(DateTime cutoffDate, CancellationToken cancellationToken = default);
}