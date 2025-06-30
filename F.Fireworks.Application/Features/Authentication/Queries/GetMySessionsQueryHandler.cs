using System.IdentityModel.Tokens.Jwt;
using Ardalis.Result;
using F.Fireworks.Application.Contracts.Persistence;
using F.Fireworks.Application.Contracts.Services;
using F.Fireworks.Application.DTOs.Authentication;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace F.Fireworks.Application.Features.Authentication.Queries;

public class GetMySessionsQueryHandler(
    IApplicationDbContext context,
    IClientIpService clientIpService,
    IGeoIpService geoIpService,
    ICurrentUserService currentUser,
    IUserAgentParserService userAgentParserService)
    : IRequestHandler<GetMySessionsQuery, Result<List<UserSessionDto>>>
{
    public async Task<Result<List<UserSessionDto>>> Handle(GetMySessionsQuery request,
        CancellationToken cancellationToken)
    {
        var userId = currentUser.UserId;
        if (userId is null) return Result<List<UserSessionDto>>.Unauthorized();
        var activeSessions = await context.RefreshTokens
            .Where(rt => rt.UserId == userId.Value && rt.RevokedOn == null && rt.Expires > DateTime.UtcNow)
            .OrderByDescending(rt => rt.CreatedOn)
            .Select(rt => new
            {
                rt.Id,
                rt.CreatedByIp,
                rt.UserAgent,
                rt.CreatedOn,
                rt.Expires,
                rt.Jti
            })
            .ToListAsync(cancellationToken);

        var sessionDtos = activeSessions.Select(s =>
        {
            var deviceInfo = userAgentParserService.Parse(s.UserAgent ?? "");
            return new UserSessionDto
            {
                Id = s.Id,
                IpAddress = s.CreatedByIp,
                Device = $"{deviceInfo.OsFamily} - {deviceInfo.BrowserFamily}",
                Location = geoIpService.GetLocation(s.CreatedByIp),
                CreatedOn = s.CreatedOn,
                ExpiresOn = s.Expires,
                IsCurrent = s.Jti == currentUser.GetClaim(JwtRegisteredClaimNames.Jti)
            };
        }).ToList();
        return Result<List<UserSessionDto>>.Success(sessionDtos);
    }
}