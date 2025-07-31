using Ardalis.Result;
using F.Fireworks.Application.Contracts.Services;
using F.Fireworks.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace F.Fireworks.Application.Features.Account.Commands;

public class ChangePwdCommandHandler(UserManager<ApplicationUser> userManager, ICurrentUserService currentUser)
    : IRequestHandler<ChangePwdCommand, Result>
{
    public async Task<Result> Handle(ChangePwdCommand request, CancellationToken cancellationToken)
    {
        var userId = currentUser.UserId;
        if (userId is null) return Result.Unauthorized("无效的用户凭证。");
        var user = await userManager.FindByIdAsync(userId.Value.ToString());
        if (user is null) return Result.Error("无法找到当前用户信息，请尝试重新登录。");

        if (!await userManager.CheckPasswordAsync(user, request.OldPassword))
            return Result.Invalid(new ValidationError("OldPassword", "旧密码不正确。"));

        var result = await userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);
        if (result.Succeeded) return Result.SuccessWithMessage("密码修改成功。");
        var errorMessages = string.Join(" ", result.Errors.Select(e => e.Description));
        return Result.Invalid(new ValidationError("NewPassword", errorMessages));
    }
}