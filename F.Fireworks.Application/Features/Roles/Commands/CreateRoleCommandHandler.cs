using Ardalis.Result;
using F.Fireworks.Application.Contracts.Services;
using F.Fireworks.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace F.Fireworks.Application.Features.Roles.Commands;

public class CreateRoleCommandHandler(RoleManager<ApplicationRole> roleManager, ICurrentUserService currentUser)
    : IRequestHandler<CreateRoleCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        var newRole = new ApplicationRole
        {
            Name = request.Name,
            Description = request.Description,
            TenantId = currentUser.TenantId ?? Guid.Empty
        };
        var result = await roleManager.CreateAsync(newRole);
        if (result.Succeeded) return Result<Guid>.Success(newRole.Id);
        var errors = result.Errors
            .Select(e => new ValidationError { Identifier = e.Code, ErrorMessage = e.Description })
            .ToList();
        return Result<Guid>.Invalid(errors);
    }
}