using F.Fireworks.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace F.Fireworks.Infrastructure.Identity;

public class TenantAwareUserValidator : UserValidator<ApplicationUser>
{
    public override async Task<IdentityResult> ValidateAsync(UserManager<ApplicationUser> manager, ApplicationUser user)
    {
        var result = await base.ValidateAsync(manager, user);
        var errors = result.Succeeded ? new List<IdentityError>() : result.Errors.ToList();
        var owner = await manager.Users.FirstOrDefaultAsync(u =>
            u.NormalizedUserName == manager.NormalizeName(user.UserName) &&
            u.TenantId == user.TenantId);
        if (owner != null && owner.Id != user.Id)
            errors.Add(new IdentityError
            {
                Code = "DuplicateUserName", Description = $"Username '{user.UserName}' is already taken in this tenant."
            });
        owner = await manager.Users.FirstOrDefaultAsync(u =>
            u.NormalizedEmail == manager.NormalizeEmail(user.Email) &&
            u.TenantId == user.TenantId);
        if (owner != null && owner.Id != user.Id)
            errors.Add(new IdentityError
                { Code = "DuplicateEmail", Description = $"Email '{user.Email}' is already taken in this tenant." });
        return errors.Count != 0 ? IdentityResult.Failed(errors.ToArray()) : IdentityResult.Success;
    }
}