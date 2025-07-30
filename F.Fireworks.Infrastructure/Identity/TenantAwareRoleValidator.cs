using F.Fireworks.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace F.Fireworks.Infrastructure.Identity;

public class TenantAwareRoleValidator : IRoleValidator<ApplicationRole>
{
    public async Task<IdentityResult> ValidateAsync(RoleManager<ApplicationRole> manager, ApplicationRole role)
    {
        var errors = new List<IdentityError>();
        if (string.IsNullOrWhiteSpace(role.Name))
        {
            errors.Add(new IdentityError { Code = "RoleNameRequired", Description = "Role name is required." });
            return IdentityResult.Failed(errors.ToArray());
        }

        var ownerRole = await manager.Roles
            .FirstOrDefaultAsync(r =>
                r.NormalizedName == manager.NormalizeKey(role.Name) &&
                r.TenantId == role.TenantId);
        if (ownerRole != null && ownerRole.Id != role.Id)
            errors.Add(new IdentityError
            {
                Code = "DuplicateRoleName", Description = $"Role name '{role.Name}' is already taken in this tenant."
            });
        return errors.Count != 0 ? IdentityResult.Failed(errors.ToArray()) : IdentityResult.Success;
    }
}