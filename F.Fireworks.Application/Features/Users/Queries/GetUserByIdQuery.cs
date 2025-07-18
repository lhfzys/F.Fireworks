using Ardalis.Result;
using F.Fireworks.Application.DTOs.Users;
using MediatR;

namespace F.Fireworks.Application.Features.Users.Queries;

public record GetUserByIdQuery(Guid Id) : IRequest<Result<UserDto>>;