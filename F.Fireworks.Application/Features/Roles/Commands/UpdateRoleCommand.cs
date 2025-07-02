using System.Text.Json.Serialization;
using Ardalis.Result;
using MediatR;

namespace F.Fireworks.Application.Features.Roles.Commands;

public record UpdateRoleCommand(
    [property: JsonIgnore] Guid Id,
    string Name,
    string? Description) : IRequest<Result>;