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
        var result = await userManager.UpdateAsync(user);

        return result.Succeeded
            ? Result.Success()
            : Result.Invalid(result.Errors.Select(e => new ValidationError(e.Code, e.Description)).ToList());
    }
}