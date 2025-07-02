using Ardalis.Result;
using F.Fireworks.Application.Contracts.Persistence;
using F.Fireworks.Domain.Identity;
using F.Fireworks.Domain.Tenants;
using F.Fireworks.Shared.Enums;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace F.Fireworks.Application.Features.Authentication.Commands;

public class RegisterUserCommandHandler(UserManager<ApplicationUser> userManager, IApplicationDbContext context)
    : IRequestHandler<RegisterUserCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var tenant = await context.Tenants
            .FirstOrDefaultAsync(t => t.Name == request.TenantName, cancellationToken);
        if (tenant is null)
        {
            tenant = new Tenant
            {
                Id = Guid.NewGuid(),
                Name = request.TenantName,
                Type = TenantType.System,
                IsActive = true
            };
            await context.Tenants.AddAsync(tenant, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }

        var user = new ApplicationUser
        {
            UserName = request.UserName,
            TenantId = tenant.Id,
            Status = UserStatus.Active
        };
        var identityResult = await userManager.CreateAsync(user, request.Password);

        if (!identityResult.Succeeded)
        {
            var errors = identityResult.Errors
                .Select(e => new ValidationError { Identifier = e.Code, ErrorMessage = e.Description })
                .ToList();
            return Result<Guid>.Invalid(errors);
        }

        await context.SaveChangesAsync(cancellationToken);
        return Result<Guid>.Success(user.Id);
    }
}