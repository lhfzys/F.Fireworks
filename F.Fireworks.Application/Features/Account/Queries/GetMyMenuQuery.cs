using Ardalis.Result;
using F.Fireworks.Application.DTOs.Account;
using MediatR;

namespace F.Fireworks.Application.Features.Account.Queries;

public record GetMyMenuQuery : IRequest<Result<List<MenuNodeDto>>>;