using System.Text;
using F.Fireworks.Infrastructure.Options;
using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using NSwag;
using OpenApiSecurityScheme = NSwag.OpenApiSecurityScheme;

namespace F.Fireworks.Api.Extensions;

public static class ServiceCollectionExtensions
{
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

        // 在这里可以链式添加所有授权策略
        services.AddAuthorizationBuilder()
            .AddPolicy("RequireAdmin", policy => policy.RequireRole("Admin"));
        // .AddPolicy("AnotherPolicy", policy => ...);

        return services;
    }

    public static IServiceCollection AddAppOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<JwtSettings>()
            .Bind(configuration.GetSection(JwtSettings.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();
        return services;
    }

    public static IServiceCollection AddApiServices(this IServiceCollection services)
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
            o.AutoTagPathSegmentIndex = 2;
        });
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
}