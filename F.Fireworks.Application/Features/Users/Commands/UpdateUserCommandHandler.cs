using Ardalis.Result;
using F.Fireworks.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace F.Fireworks.Application.Features.Users.Commands;

public class UpdateUserCommandHandler(UserManager<ApplicationUser> userManager)
    : IRequestHandler<UpdateUserCommand, Result>
{
    public async Task<Result> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(request.Id.ToString());
        if (user is null) return Result.NotFound("用户不存在或已被删除");
        user.Status = request.Status;
        var result = await userManager.UpdateAsync(user);

        return result.Succeeded
            ? Result.Success()
            : Result.Invalid(result.Errors.Select(e => new ValidationError(e.Code, e.Description)).ToList());
    }
}