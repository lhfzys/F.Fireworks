using Ardalis.Result;
using F.Fireworks.Application.DTOs.Authentication;
using MediatR;

namespace F.Fireworks.Application.Features.Authentication.Queries;

public record GetUserSessionsAsAdminQuery(Guid UserId) : IRequest<Result<List<UserSessionDto>>>;