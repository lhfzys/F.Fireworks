using System.Security.Claims;
using F.Fireworks.Domain.Identity;

namespace F.Fireworks.Application.Contracts.Identity;

public interface ITokenService
{
    string CreateToken(ApplicationUser user, IEnumerable<string> roles);
    ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
}