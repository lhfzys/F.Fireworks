using Ardalis.Result;
using MediatR;

namespace F.Fireworks.Application.Features.Authentication.Commands;

public record LogoutCommand(string RefreshToken) : IRequest<Result>;