using Ardalis.Result;
using F.Fireworks.Application.DTOs.Auditing;
using F.Fireworks.Application.DTOs.Common;
using MediatR;

namespace F.Fireworks.Application.Features.Auditing.Queries;

public record GetAuditLogsQuery(AuditLogFilter Filter) : IRequest<Result<PaginatedList<AuditLogDto>>>;