using Ardalis.Result;
using F.Fireworks.Application.DTOs.Authentication;
using MediatR;

namespace F.Fireworks.Application.Features.Authentication.Commands;

public record RefreshTokenCommand : IRequest<Result<LoginResponse>>;