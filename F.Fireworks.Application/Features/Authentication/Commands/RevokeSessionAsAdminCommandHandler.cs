using Ardalis.Result;
using F.Fireworks.Application.Contracts.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace F.Fireworks.Application.Features.Authentication.Commands;

public class RevokeSessionAsAdminCommandHandler(IApplicationDbContext context)
    : IRequestHandler<RevokeSessionAsAdminCommand, Result>
{
    // private readonly IHubContext<NotificationHub> _hubContext; // 稍后集成 SignalR

    //, IHubContext<NotificationHub> hubContext)
    // _hubContext = hubContext;

    public async Task<Result> Handle(RevokeSessionAsAdminCommand request, CancellationToken cancellationToken)
    {
        var storedToken = await context.RefreshTokens
            .SingleOrDefaultAsync(rt => rt.Id == request.SessionId, cancellationToken);

        if (storedToken is null || !storedToken.IsActive)
            return Result.SuccessWithMessage("Session was already inactive.");

        storedToken.RevokedOn = DateTime.UtcNow;
        await context.SaveChangesAsync(cancellationToken);

        // TODO: 稍后在这里添加 SignalR 推送逻辑
        // await _hubContext.Clients.Group($"user_{storedToken.UserId}")
        //     .SendAsync("ForceLogout", "Your session has been terminated by an administrator.", cancellationToken);

        return Result.SuccessWithMessage("Session successfully revoked.");
    }
}