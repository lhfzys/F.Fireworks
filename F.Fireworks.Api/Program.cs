using F.Fireworks.Api.Extensions;
using F.Fireworks.Api.Middlewares;
using F.Fireworks.Application;
using F.Fireworks.Infrastructure;
using F.Fireworks.Infrastructure.BackgroundJobs;
using FastEndpoints;
using Hangfire;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);
// --- 服务注册区域 ---
// 注册强类型配置选项
builder.Services.AddAppOptions(builder.Configuration);
builder.Services.AddApiServices(builder.Configuration);
builder.Services.AddHttpContextAccessor();
// 注册各层的服务
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
// 注册认证服务
builder.Services.AddAuth(builder.Configuration);
builder.Services.AddOpenApi();
builder.Services.AddRateLimiting();
// 注册跨域
builder.Services.AddCorsPolicy(builder.Configuration);
var app = builder.Build();
app.UseHttpsRedirection();
app.UseSecurityHeaders();
app.UseCors("DefaultCorsPolicy");
app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<AuditLogMiddleware>();
app.UseFastEndpoints(c => { c.Endpoints.RoutePrefix = "api"; });
app.UseSwaggerDocumentation();
app.MapHealthChecks("/healthz", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
app.MapHealthChecksUI(options => { options.UIPath = "/health-ui"; });
await app.SeedDataAsync();

app.UseHangfireDashboard();
RecurringJob.AddOrUpdate<AuditLogCleanupJob>(
    "AuditLogCleanup",
    job => job.Run(),
    Cron.Daily());

app.Run();