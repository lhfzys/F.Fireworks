using Ardalis.Result;
using F.Fireworks.Application.Contracts.Services;
using F.Fireworks.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace F.Fireworks.Application.Features.Users.Commands;

public class UpdateUserCommandHandler(UserManager<ApplicationUser> userManager, ICurrentUserService currentUser)
    : IRequestHandler<UpdateUserCommand, Result>
{
    public async Task<Result> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        // 1. 根据ID从数据库中完整地加载出要被修改的用户实体
        var user = await userManager.FindByIdAsync(request.Id.ToString());
        if (user is null) return Result.NotFound("用户不存在或已被删除");
        // 2: 执行核心的"租户所有权"安全检查
        if (!currentUser.IsInRole("SuperAdmin") && user.TenantId != currentUser.TenantId)
            return Result.NotFound("用户不存在或已被删除");
        user.Status = request.Status;

        var userNameResult = await userManager.SetUserNameAsync(user, request.UserName);
        if (!userNameResult.Succeeded)
            return Result.Invalid(
                userNameResult.Errors.Select(e => new ValidationError(e.Code, e.Description)).ToList());
        if (user.Email != request.Email)
        {
            var emailResult = await userManager.SetEmailAsync(user, request.Email);
            if (!emailResult.Succeeded)
                return Result.Invalid(emailResult.Errors.Select(e => new ValidationError(e.Code, e.Description))
                    .ToList());
        }

        var updateResult = await userManager.UpdateAsync(user);

        return updateResult.Succeeded
            ? Result.Success()
            : Result.Invalid(updateResult.Errors.Select(e => new ValidationError(e.Code, e.Description)).ToList());
    }
}