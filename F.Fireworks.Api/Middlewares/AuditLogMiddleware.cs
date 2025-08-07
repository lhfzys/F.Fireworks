using System.Diagnostics;
using System.Text;
using F.Fireworks.Application.Contracts.Persistence;
using F.Fireworks.Application.Contracts.Services;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.EntityFrameworkCore;

namespace F.Fireworks.Api.Middlewares;

public class AuditLogMiddleware(RequestDelegate next)
{
    private const int MaxBodySizeToLog = 4096;

    public async Task InvokeAsync(HttpContext context, IApplicationDbContext dbContext, IAuditService auditService,
        ICurrentUserService currentUser, IDataSanitizer sanitizer)
    {
        if (HttpMethods.IsGet(context.Request.Method) || HttpMethods.IsHead(context.Request.Method) ||
            HttpMethods.IsOptions(context.Request.Method))
        {
            await next(context);
            return;
        }

        var stopwatch = Stopwatch.StartNew();
        var requestBody = await GetRequestPayloadAsync(context.Request);

        var originalBodyStream = context.Response.Body;
        using var responseBody = new MemoryStream();
        context.Response.Body = responseBody;

        await next(context);
        stopwatch.Stop();

        var responseBodyContent = await GetResponsePayloadAsync(context.Response);
        await responseBody.CopyToAsync(originalBodyStream);
        string? tenantName = null;
        if (currentUser.TenantId.HasValue)
            tenantName = await dbContext.Tenants
                .Where(t => t.Id == currentUser.TenantId.Value)
                .Select(t => t.Name)
                .FirstOrDefaultAsync();
        var auditInfo = new AuditInfo(
            currentUser.UserId,
            context.User.Identity?.Name,
            currentUser.TenantId ?? Guid.Empty,
            tenantName,
            context.Request.Path,
            context.Request.GetDisplayUrl(),
            context.Request.Method,
            context.Response.StatusCode,
            stopwatch.ElapsedMilliseconds,
            context.Connection.RemoteIpAddress?.ToString() ?? "N/A",
            context.Request.Headers["User-Agent"].ToString(),
            sanitizer.Sanitize(requestBody),
            sanitizer.Sanitize(responseBodyContent)
        );

        await auditService.AuditAsync(auditInfo, context.RequestAborted);
    }

    private async Task<string> GetRequestPayloadAsync(HttpRequest request)
    {
        if (request.ContentType?.Contains("multipart/form-data") ?? false)
        {
            var fileNames = request.Form.Files.Select(f => f.FileName);
            return $"{{ \"isFileUpload\": true, \"files\": [\"{string.Join("\", \"", fileNames)}\"] }}";
        }

        if (request.ContentLength > MaxBodySizeToLog)
            return $"{{ \"payloadTooLarge\": true, \"size\": {request.ContentLength} }}";

        request.EnableBuffering();
        using var reader = new StreamReader(request.Body, Encoding.UTF8, leaveOpen: true);
        var body = await reader.ReadToEndAsync();
        request.Body.Position = 0;
        return body;
    }

    private async Task<string> GetResponsePayloadAsync(HttpResponse response)
    {
        if (response.ContentType?.Contains("application/octet-stream") ?? false) return "{ \"isFileDownload\": true }";

        if (response.ContentLength > MaxBodySizeToLog)
            return $"{{ \"payloadTooLarge\": true, \"size\": {response.ContentLength} }}";

        response.Body.Seek(0, SeekOrigin.Begin);
        var text = await new StreamReader(response.Body).ReadToEndAsync();
        response.Body.Seek(0, SeekOrigin.Begin);
        return text;
    }
}