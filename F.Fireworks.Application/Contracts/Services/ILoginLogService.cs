namespace F.Fireworks.Application.Contracts.Services;

public interface ILoginLogService
{
    Task LogAsync(Guid userId, Guid tenantId, string email, bool wasSuccessful, string? failureReason,
        CancellationToken cancellationToken = default);
}