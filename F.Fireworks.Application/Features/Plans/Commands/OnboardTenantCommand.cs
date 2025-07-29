using Ardalis.Result;
using F.Fireworks.Shared.Enums;
using MediatR;

namespace F.Fireworks.Application.Features.Plans.Commands;

public record OnboardTenantCommand(
    string TenantName,
    TenantType TenantType,
    Guid? PlanId,
    string AdminEmail,
    string AdminUserName,
    string AdminPassword
) : IRequest<Result>;