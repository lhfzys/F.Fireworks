using Ardalis.Result;
using F.Fireworks.Application.Contracts.Persistence;
using F.Fireworks.Application.Contracts.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace F.Fireworks.Application.Features.Users.Commands;

public class DeleteUserCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser)
    : IRequestHandler<DeleteUserCommand, Result>
{
    public async Task<Result> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);
        if (user is null) return Result.NotFound("用户不存在或已被删除");
        if (!currentUser.IsInRole("SuperAdmin") && user.TenantId != currentUser.TenantId)
            return Result.Forbidden("用户不存在或已被删除");
        context.Users.Remove(user);
        await context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}