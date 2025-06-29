using F.Fireworks.Application.Contracts.Persistence;
using F.Fireworks.Application.Contracts.Services;
using F.Fireworks.Domain.Logging;
using Microsoft.AspNetCore.Http;
using UAParser;

namespace F.Fireworks.Infrastructure.Services;

public class LoginLogService(
    IApplicationDbContext context,
    IClientIpService ipService,
    IGeoIpService geoIpService,
    IHttpContextAccessor httpContextAccessor)
    : ILoginLogService
{
    public async Task LogAsync(Guid? userId, Guid? tenantId, string email, bool wasSuccessful, string? failureReason,
        CancellationToken cancellationToken = default)
    {
        var ipAddress = ipService.GetClientIp();
        var location = geoIpService.GetLocation(ipAddress);
        var userAgent = httpContextAccessor.HttpContext?.Request.Headers.UserAgent.ToString() ?? "N/A";

        var uaParser = Parser.GetDefault();
        var clientInfo = uaParser.Parse(userAgent);
        var device = clientInfo.Device.Family ?? "未知设备";

        var log = new UserLoginLog
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            TenantId = tenantId,
            IpAddress = ipAddress,
            Location = location,
            Device = device,
            UserAgent = userAgent,
            WasSuccessful = wasSuccessful,
            FailureReason = failureReason
        };

        await context.UserLoginLogs.AddAsync(log, cancellationToken);
    }
}