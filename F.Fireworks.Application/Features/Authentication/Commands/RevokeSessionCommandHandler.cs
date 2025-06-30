using Ardalis.Result;
using F.Fireworks.Application.Contracts.Persistence;
using F.Fireworks.Application.Contracts.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace F.Fireworks.Application.Features.Authentication.Commands;

public class RevokeSessionCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser)
    : IRequestHandler<RevokeSessionCommand, Result>
{
    public async Task<Result> Handle(RevokeSessionCommand request, CancellationToken cancellationToken)
    {
        var currentUserId = currentUser.UserId;
        if (currentUserId is null)
            return Result.Error("Unable to determine current user.");

        var storedToken = await context.RefreshTokens
            .SingleOrDefaultAsync(rt => rt.Id == request.SessionId, cancellationToken);

        if (storedToken is null) return Result.NotFound("Session not found.");

        if (storedToken.UserId != currentUserId) return Result.Forbidden();

        if (!storedToken.IsActive)
            return Result.SuccessWithMessage("Session was already inactive.");

        storedToken.RevokedOn = DateTime.UtcNow;

        await context.SaveChangesAsync(cancellationToken);

        return Result.SuccessWithMessage("Session successfully revoked.");
    }
}