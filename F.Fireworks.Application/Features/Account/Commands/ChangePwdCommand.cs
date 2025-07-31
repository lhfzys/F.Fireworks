using Ardalis.Result;
using MediatR;

namespace F.Fireworks.Application.Features.Account.Commands;

public record ChangePwdCommand(string OldPassword, string NewPassword, string ConfirmedPassword)
    : IRequest<Result>;