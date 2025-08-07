using Ardalis.Result;
using F.Fireworks.Application.Common.Extensions;
using F.Fireworks.Application.Contracts.Persistence;
using F.Fireworks.Application.Contracts.Services;
using F.Fireworks.Application.DTOs.Auditing;
using F.Fireworks.Application.DTOs.Common;
using F.Fireworks.Domain.Constants;
using F.Fireworks.Domain.Logging;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace F.Fireworks.Application.Features.Auditing.Queries;

public class GetAuditLogsQueryHandler(IApplicationDbContext context, ICurrentUserService currentUser)
    : IRequestHandler<GetAuditLogsQuery, Result<PaginatedList<AuditLogDto>>>
{
    public async Task<Result<PaginatedList<AuditLogDto>>> Handle(GetAuditLogsQuery request,
        CancellationToken cancellationToken)
    {
        var query = context.AuditLogs.AsNoTracking();

        // --- 1. 租户隔离逻辑 ---
        if (!currentUser.IsInRole(RoleConstants.SuperAdmin))
            query = query.Where(a => a.TenantId == currentUser.TenantId);

        // --- 2. 手动处理通用筛选器（如日期范围） ---
        if (request.Filter.DateFrom.HasValue)
            query = query.Where(a => a.Timestamp >= request.Filter.DateFrom.Value.ToUniversalTime());
        if (request.Filter.DateTo.HasValue)
            query = query.Where(a => a.Timestamp < request.Filter.DateTo.Value.ToUniversalTime().AddDays(1));

        // 3. 应用通用的筛选、排序和分页
        var paginatedResult = await query
            .ApplyFiltering(request.Filter)
            .ApplySort(request.Filter.SortField ?? nameof(AuditLog.Timestamp),
                request.Filter.SortOrder ?? "descend")
            .ProjectToType<AuditLogDto>()
            .ToPaginatedListAsync(request.Filter.PageNumber, request.Filter.PageSize, cancellationToken);

        return Result.Success(paginatedResult);
    }
}