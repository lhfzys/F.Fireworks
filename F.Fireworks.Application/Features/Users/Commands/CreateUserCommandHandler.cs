using Ardalis.Result;
using F.Fireworks.Application.Contracts.Services;
using F.Fireworks.Domain.Constants;
using F.Fireworks.Domain.Identity;
using F.Fireworks.Shared.Enums;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace F.Fireworks.Application.Features.Users.Commands;

public class CreateUserCommandHandler(
    UserManager<ApplicationUser> userManager,
    ICurrentUserService currentUser) : IRequestHandler<CreateUserCommand, Result>
{
    public async Task<Result> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        Guid targetTenantId;
        if (currentUser.IsInRole(RoleConstants.SuperAdmin))
        {
            if (!request.TenantId.HasValue) return Result.Invalid(new ValidationError("TenantId", "您必须指定一个租户"));
            targetTenantId = request.TenantId.Value;
        }
        else
        {
            var tenantId = currentUser.TenantId;
            if (tenantId is null) return Result.Forbidden("操作无效，租户不能为空");
            targetTenantId = tenantId.Value;
        }

        var newUser = new ApplicationUser
        {
            UserName = request.UserName,
            Email = request.Email,
            Status = UserStatus.Active,
            TenantId = targetTenantId,
            MustChangePassword = true
        };
        var result = await userManager.CreateAsync(newUser, request.Password);
        if (result.Succeeded) return Result.Success();
        var errors = result.Errors
            .Select(e => new ValidationError { Identifier = e.Code, ErrorMessage = e.Description })
            .ToList();
        return Result.Invalid(errors);
    }
}