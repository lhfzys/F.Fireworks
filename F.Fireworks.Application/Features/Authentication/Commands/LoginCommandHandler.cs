using System.Security.Cryptography;
using Ardalis.Result;
using F.Fireworks.Application.Contracts.Identity;
using F.Fireworks.Application.Contracts.Persistence;
using F.Fireworks.Application.Contracts.Services;
using F.Fireworks.Application.DTOs.Authentication;
using F.Fireworks.Domain.Identity;
using F.Fireworks.Shared.Enums;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace F.Fireworks.Application.Features.Authentication.Commands;

public class LoginCommandHandler(
    UserManager<ApplicationUser> userManager,
    ITokenService tokenService,
    ILoginLogService loginLogService,
    IClientIpService clientIpService,
    IApplicationDbContext context)
    : IRequestHandler<LoginDto.LoginCommand, Result<LoginDto.LoginResponse>>
{
    public async Task<Result<LoginDto.LoginResponse>> Handle(LoginDto.LoginCommand request,
        CancellationToken cancellationToken)
    {
        var user = await userManager.FindByNameAsync(request.UserName);
        if (user is null || !await userManager.CheckPasswordAsync(user, request.Password))
        {
            await loginLogService.LogAsync(user?.Id, user?.TenantId, request.UserName,
                false, "用户名或密码错误",
                cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
            return Result<LoginDto.LoginResponse>.NotFound("用户名或密码错误");
        }

        if (user.Status != UserStatus.Active)
        {
            await loginLogService.LogAsync(user.Id, user.TenantId, request.UserName, false, "账号未激活或已被封禁",
                cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
            return Result<LoginDto.LoginResponse>.NotFound("账号未激活或已被封禁");
        }

        await loginLogService.LogAsync(user.Id, user.TenantId, request.UserName, true, null, cancellationToken);

        var roles = await userManager.GetRolesAsync(user);
        var accessToken = tokenService.CreateToken(user, roles);
        var refreshTokenString = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        var refreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            TenantId = user.TenantId,
            Token = refreshTokenString,
            Expires = DateTime.UtcNow.AddDays(30),
            CreatedByIp = clientIpService.GetClientIp()
        };
        await context.RefreshTokens.AddAsync(refreshToken, cancellationToken);
        var response = new LoginDto.LoginResponse(accessToken, refreshToken.Token);
        await context.SaveChangesAsync(cancellationToken);
        return Result<LoginDto.LoginResponse>.Success(response);
    }
}