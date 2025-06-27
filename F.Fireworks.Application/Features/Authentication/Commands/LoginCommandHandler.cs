using System.Security.Cryptography;
using Ardalis.Result;
using F.Fireworks.Application.Contracts.Identity;
using F.Fireworks.Application.DTOs.Authentication;
using F.Fireworks.Domain.Identity;
using F.Fireworks.Shared.Enums;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace F.Fireworks.Application.Features.Authentication.Commands;

public class LoginCommandHandler(
    UserManager<ApplicationUser> userManager,
    ITokenService tokenService)
    : IRequestHandler<LoginDto.LoginCommand, Result<LoginDto.LoginResponse>>
{
    public async Task<Result<LoginDto.LoginResponse>> Handle(LoginDto.LoginCommand request,
        CancellationToken cancellationToken)
    {
        var user = await userManager.FindByNameAsync(request.UserName);
        if (user is null || !await userManager.CheckPasswordAsync(user, request.Password))
            return Result<LoginDto.LoginResponse>.NotFound("用户名或密码错误");
        if (user.Status != UserStatus.Active)
            return Result<LoginDto.LoginResponse>.NotFound("账号未激活或已被封禁");
        var roles = await userManager.GetRolesAsync(user);
        var accessToken = tokenService.CreateToken(user, roles);
        // TODO: 保存 Refresh Token 到数据库
        var refreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        var response = new LoginDto.LoginResponse(accessToken, refreshToken);
        return Result<LoginDto.LoginResponse>.Success(response);
    }
}