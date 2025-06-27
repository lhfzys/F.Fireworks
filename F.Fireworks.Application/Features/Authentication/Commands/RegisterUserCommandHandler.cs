using Ardalis.Result;
using F.Fireworks.Application.Contracts.Persistence;
using F.Fireworks.Domain.Identity;
using F.Fireworks.Shared.Enums;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace F.Fireworks.Application.Features.Authentication.Commands;

public class RegisterUserCommandHandler(UserManager<ApplicationUser> userManager, IApplicationDbContext context)
    : IRequestHandler<RegisterUserCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var user = new ApplicationUser
        {
            UserName = request.UserName,
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