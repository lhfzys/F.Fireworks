using Ardalis.Result;
using F.Fireworks.Application.Contracts.Services;
using F.Fireworks.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace F.Fireworks.Application.Features.Users.Commands;

public class ResetPasswordCommandHandler(UserManager<ApplicationUser> userManager, ICurrentUserService currentUser)
    : IRequestHandler<ResetPasswordCommand, Result>
{
    public async Task<Result> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(request.Id.ToString());
        if (user is null) return Result.NotFound("用户不存在或已被删除");
        if (!currentUser.IsInRole("SuperAdmin") && user.TenantId != currentUser.TenantId)
            return Result.Forbidden("您无权操作");
        var token = await userManager.GeneratePasswordResetTokenAsync(user);
        var result = await userManager.ResetPasswordAsync(user, token, request.NewPassword);
        return result.Succeeded
            ? Result.Success()
            : Result.Invalid(result.Errors.Select(e => new ValidationError
                { Identifier = e.Code, ErrorMessage = e.Description }));
    }
}