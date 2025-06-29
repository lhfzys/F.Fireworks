using Ardalis.Result;
using F.Fireworks.Application.DTOs.Common;
using F.Fireworks.Application.DTOs.Users;
using MediatR;

namespace F.Fireworks.Application.Features.Users.Queries;

public record GetUsersQuery(UserFilter Filter) : IRequest<Result<PaginatedList<UserDto>>>;