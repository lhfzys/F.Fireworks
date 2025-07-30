using Ardalis.Result;
using F.Fireworks.Application.Contracts.Persistence;
using F.Fireworks.Application.Contracts.Services;
using F.Fireworks.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace F.Fireworks.Application.Features.Users.Commands;

public class DeleteUserCommandHandler(
    IApplicationDbContext context,
    UserManager<ApplicationUser> userManager,
    ICurrentUserService currentUser)
    : IRequestHandler<DeleteUserCommand, Result>
{
    public async Task<Result> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(request.Id.ToString());
        if (user is null || (!currentUser.IsInRole("SuperAdmin") && user.TenantId != currentUser.TenantId))
            return Result.NotFound("用户不存在或已被删除");
        context.Users.Remove(user);
        await context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}