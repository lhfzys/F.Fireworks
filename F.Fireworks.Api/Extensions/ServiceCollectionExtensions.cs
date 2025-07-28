using System.Text;
using System.Threading.RateLimiting;
using F.Fireworks.Infrastructure.Auth;
using F.Fireworks.Infrastructure.Options;
using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using NSwag;
using OpenApiSecurityScheme = NSwag.OpenApiSecurityScheme;

namespace F.Fireworks.Api.Extensions;

public static class ServiceCollectionExtensions
{
    private const string DefaultCorsPolicyName = "DefaultCorsPolicy";

    public static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = new JwtSettings();
        configuration.Bind(JwtSettings.SectionName, jwtSettings);

        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),
                    ClockSkew = TimeSpan.Zero
                };
            });

        services.AddAuthorization();
        services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
        return services;
    }

    public static IServiceCollection AddAppOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<JwtSettings>()
            .Bind(configuration.GetSection(JwtSettings.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();
        services.AddOptions<InitialSetupSettings>()
            .Bind(configuration.GetSection(InitialSetupSettings.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();
        return services;
    }

    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddFastEndpoints();
        services.AddEndpointsApiExplorer();
        services.SwaggerDocument(o =>
        {
            o.DocumentSettings = s =>
            {
                s.DocumentName = "Initial-Release";
                s.Title = "Fireworks.Api";
                s.Version = "v1.0";
                s.Description = "Api接口文档";
                s.AddAuth("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = OpenApiSecuritySchemeType.Http,
                    Scheme = JwtBearerDefaults.AuthenticationScheme,
                    BearerFormat = "JWT",
                    Description = "输入 'Bearer {token}'"
                });
            };
            o.AutoTagPathSegmentIndex = 0;
        });
        services.AddHealthChecks()
            .AddNpgSql(
                configuration.GetConnectionString("PostgresSqlConnection") ?? string.Empty,
                name: "PostgreSQL Database",
                failureStatus: HealthStatus.Unhealthy, // 当检查失败时报告的状态
                tags: ["database", "critical"]);
        return services;
    }

    public static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app)
    {
        app.UseSwaggerGen();
        // .UseSwaggerUI(c =>
        // {
        //     c.RoutePrefix = "api-docs";
        //     c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        //     c.DocExpansion(DocExpansion.List);
        //     c.DisplayRequestDuration();
        // });
        return app;
    }

    public static IServiceCollection AddCorsPolicy(this IServiceCollection services, IConfiguration configuration)
    {
        var corsSettings = configuration.GetSection("CorsSettings");
        var allowedOrigins = corsSettings.GetValue<string>("AllowedOrigins")?.Split(',') ?? [];
        services.AddCors(options =>
        {
            options.AddPolicy(DefaultCorsPolicyName, policy =>
            {
                policy
                    .WithOrigins(allowedOrigins)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            });
        });
        return services;
    }


    public static IServiceCollection AddRateLimiting(this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            options.AddFixedWindowLimiter("fixed", o =>
            {
                o.PermitLimit = 10;
                o.Window = TimeSpan.FromSeconds(10);
                o.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                o.QueueLimit = 2;
            });
            options.AddSlidingWindowLimiter("sliding", o =>
            {
                o.PermitLimit = 15;
                o.Window = TimeSpan.FromSeconds(10);
                o.SegmentsPerWindow = 5;
                o.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                o.QueueLimit = 2;
            });
            options.AddTokenBucketLimiter("token", o =>
            {
                o.TokenLimit = 20;
                o.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                o.QueueLimit = 1;
                o.TokensPerPeriod = 10;
                o.ReplenishmentPeriod = TimeSpan.FromMinutes(1);
            });
            options.OnRejected = (context, cancellationToken) =>
            {
                context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                context.HttpContext.Response.WriteAsync("Too many requests. Please try again later.",
                    cancellationToken);
                return new ValueTask();
            };
        });
        return services;
    }
}