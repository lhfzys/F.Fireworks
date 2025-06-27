using System.Security.Cryptography;
using Ardalis.Result;
using F.Fireworks.Application.Contracts.Identity;
using F.Fireworks.Application.Contracts.Persistence;
using F.Fireworks.Application.DTOs.Authentication;
using F.Fireworks.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace F.Fireworks.Application.Features.Authentication.Commands;

public class RefreshTokenCommandHandler(
    IApplicationDbContext context,
    UserManager<ApplicationUser> userManager,
    ITokenService tokenService)
    : IRequestHandler<RefreshTokenCommand, Result<LoginDto.LoginResponse>>
{
    public async Task<Result<LoginDto.LoginResponse>> Handle(RefreshTokenCommand request,
        CancellationToken cancellationToken)
    {
        var principal = tokenService.GetPrincipalFromExpiredToken(request.AccessToken);
        if (principal?.Claims.FirstOrDefault(c => c.Type == "uid")?.Value is not { } userIdString ||
            !Guid.TryParse(userIdString, out var userId)) return Result<LoginDto.LoginResponse>.Unauthorized();

        var storedToken = await context.RefreshTokens.Include(rt => rt.User)
            .SingleOrDefaultAsync(rt => rt.Token == request.RefreshToken && rt.UserId == userId, cancellationToken);

        if (storedToken is null || storedToken.IsRevoked || storedToken.IsExpired)
            return Result<LoginDto.LoginResponse>.Unauthorized();

        storedToken.RevokedOn = DateTime.UtcNow;

        var roles = await userManager.GetRolesAsync(storedToken.User);
        var newAccessToken = tokenService.CreateToken(storedToken.User, roles);

        var newRefreshTokenString = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        var newRefreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = storedToken.UserId,
            TenantId = storedToken.TenantId,
            Token = newRefreshTokenString,
            Expires = DateTime.UtcNow.AddDays(30),
            CreatedByIp = storedToken.CreatedByIp
        };
        await context.RefreshTokens.AddAsync(newRefreshToken, cancellationToken);

        await context.SaveChangesAsync(cancellationToken);

        var response = new LoginDto.LoginResponse(newAccessToken, newRefreshToken.Token);
        return Result<LoginDto.LoginResponse>.Success(response);
    }
}