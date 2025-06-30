using Ardalis.Result;
using F.Fireworks.Application.DTOs.Account;
using MediatR;

namespace F.Fireworks.Application.Features.Account.Queries;

public record GetMyProfileQuery : IRequest<Result<UserProfileDto>>;