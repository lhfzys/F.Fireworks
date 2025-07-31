using Ardalis.Result;
using MediatR;

namespace F.Fireworks.Application.Features.Users.Commands;

public record ResetPasswordCommand(Guid Id, string NewPassword) : IRequest<Result>;