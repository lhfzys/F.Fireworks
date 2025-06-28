using Ardalis.Result;
using F.Fireworks.Application.DTOs.Roles;
using F.Fireworks.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace F.Fireworks.Application.Features.Roles.Queries;

public class GetAllRolesQueryHandler(RoleManager<ApplicationRole> roleManager)
    : IRequestHandler<GetAllRolesQuery, Result<List<RoleDto>>>
{
    public async Task<Result<List<RoleDto>>> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
    {
        var roles = await roleManager.Roles
            .AsNoTracking()
            .ToListAsync(cancellationToken);
        var roleDtos = roles
            .Select(r => new RoleDto(r.Id, r.Name, r.Description))
            .ToList();
        return Result<List<RoleDto>>.Success(roleDtos);
    }
}