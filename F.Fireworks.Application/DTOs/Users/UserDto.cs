using F.Fireworks.Shared.Enums;

namespace F.Fireworks.Application.DTOs.Users;

public record UserDto(Guid Id, string UserName, UserStatus Status, DateTime CreatedOn)
{
}