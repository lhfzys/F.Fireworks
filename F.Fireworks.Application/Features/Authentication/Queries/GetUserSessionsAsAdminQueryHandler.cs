using Ardalis.Result;
using F.Fireworks.Application.Contracts.Persistence;
using F.Fireworks.Application.Contracts.Services;
using F.Fireworks.Application.DTOs.Authentication;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace F.Fireworks.Application.Features.Authentication.Queries;

public class
    GetUserSessionsAsAdminQueryHandler : IRequestHandler<GetUserSessionsAsAdminQuery, Result<List<UserSessionDto>>>
{
    private readonly IApplicationDbContext _context;
    private readonly IGeoIpService _geoIpService;
    private readonly IUserAgentParserService _userAgentParserService;

    public GetUserSessionsAsAdminQueryHandler(IApplicationDbContext context, IGeoIpService geoIpService,
        IUserAgentParserService userAgentParserService)
    {
        _context = context;
        _geoIpService = geoIpService;
        _userAgentParserService = userAgentParserService;
    }

    public async Task<Result<List<UserSessionDto>>> Handle(GetUserSessionsAsAdminQuery request,
        CancellationToken cancellationToken)
    {
        if (!await _context.Users.AnyAsync(u => u.Id == request.UserId, cancellationToken))
            return Result.NotFound("用户不存在");
        var sessions = await _context.RefreshTokens
            .Where(rt => rt.UserId == request.UserId && rt.RevokedOn == null && rt.Expires > DateTime.UtcNow)
            .OrderByDescending(rt => rt.CreatedOn)
            .Select(rt => new { rt.Id, rt.CreatedByIp, rt.UserAgent, rt.CreatedOn, rt.Expires })
            .ToListAsync(cancellationToken);
        var sessionDtos = sessions.Select(s =>
        {
            var deviceInfo = _userAgentParserService.Parse(s.UserAgent ?? "");
            return new UserSessionDto
            {
                Id = s.Id,
                IpAddress = s.CreatedByIp,
                Device = $"{deviceInfo.OsFamily} / {deviceInfo.BrowserFamily}",
                Location = _geoIpService.GetLocation(s.CreatedByIp),
                CreatedOn = s.CreatedOn,
                ExpiresOn = s.Expires,
                IsCurrent = false
            };
        }).ToList();
        return Result.Success(sessionDtos);
    }
}