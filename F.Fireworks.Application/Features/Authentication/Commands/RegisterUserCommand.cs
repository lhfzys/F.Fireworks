using Ardalis.Result;
using MediatR;

namespace F.Fireworks.Application.Features.Authentication.Commands;

public record RegisterUserCommand(
    string UserName,
    string Password,
    string TenantName) : IRequest<Result<Guid>>;