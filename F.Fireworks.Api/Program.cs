using F.Fireworks.Api.Extensions;
using F.Fireworks.Api.Middlewares;
using F.Fireworks.Application;
using F.Fireworks.Infrastructure;
using FastEndpoints;

var builder = WebApplication.CreateBuilder(args);
// --- 服务注册区域 ---
// 注册强类型配置选项
builder.Services.AddAppOptions(builder.Configuration);
builder.Services.AddApiServices();
builder.Services.AddHttpContextAccessor();
// 注册各层的服务
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
// 注册认证服务
builder.Services.AddAuth(builder.Configuration);
builder.Services.AddOpenApi();

var app = builder.Build();
app.UseMiddleware<AuditLogMiddleware>();
app.UseSwaggerDocumentation();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.UseFastEndpoints(c => { c.Endpoints.RoutePrefix = "api"; });
app.Run();