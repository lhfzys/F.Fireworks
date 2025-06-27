using Ardalis.Result;
using F.Fireworks.Application.Contracts.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace F.Fireworks.Application.Features.Authentication.Commands;

public class LogoutCommandHandler(IApplicationDbContext context) : IRequestHandler<LogoutCommand, Result>
{
    public async Task<Result> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        var storedToken = await context.RefreshTokens
            .SingleOrDefaultAsync(rt => rt.Token == request.RefreshToken, cancellationToken);
        if (storedToken is null || storedToken.IsActive == false) return Result.Success();
        storedToken.RevokedOn = DateTime.UtcNow;
        await context.SaveChangesAsync(cancellationToken);

        return Result.SuccessWithMessage("Successfully logged out.");
    }
}