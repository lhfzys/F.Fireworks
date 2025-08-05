using Ardalis.Result;
using MediatR;

namespace F.Fireworks.Application.Features.Users.Commands;

public record CreateUserCommand(string UserName, string Email, string Password, Guid? TenantId) : IRequest<Result>;