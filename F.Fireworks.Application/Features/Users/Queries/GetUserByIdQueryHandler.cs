using Ardalis.Result;
using F.Fireworks.Application.Contracts.Persistence;
using F.Fireworks.Application.Contracts.Services;
using F.Fireworks.Application.DTOs.Users;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace F.Fireworks.Application.Features.Users.Queries;

public class GetUserByIdQueryHandler(IApplicationDbContext context, ICurrentUserService currentUser)
    : IRequestHandler<GetUserByIdQuery, Result<UserDto>>
{
    public async Task<Result<UserDto>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);
        if (user is null || (!currentUser.IsInRole("SuperAdmin") && user.TenantId != currentUser.TenantId))
            return Result.NotFound("用户不存在");

        var userDto = user.Adapt<UserDto>();
        return Result.Success(userDto);
    }
}