using F.Fireworks.Application.Contracts.Identity;
using F.Fireworks.Application.Contracts.Persistence;
using F.Fireworks.Application.Contracts.Services;
using F.Fireworks.Domain.Identity;
using F.Fireworks.Infrastructure.Identity;
using F.Fireworks.Infrastructure.Persistence;
using F.Fireworks.Infrastructure.Persistence.Seeders;
using F.Fireworks.Infrastructure.Services;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace F.Fireworks.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("PostgresSqlConnection"),
                b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

        services.AddIdentityCore<ApplicationUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 6;
                options.User.RequireUniqueEmail = false;

                options.Lockout.AllowedForNewUsers = true;
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
            })
            .AddRoles<ApplicationRole>()
            .AddSignInManager<SignInManager<ApplicationUser>>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        // 覆盖Identity默认验证器
        var defaultUserValidator =
            services.FirstOrDefault(d => d.ImplementationType == typeof(UserValidator<ApplicationUser>));
        if (defaultUserValidator != null) services.Remove(defaultUserValidator);
        services.AddScoped<IUserValidator<ApplicationUser>, TenantAwareUserValidator>();
        var defaultRoleValidator =
            services.FirstOrDefault(d => d.ImplementationType == typeof(RoleValidator<ApplicationRole>));
        if (defaultRoleValidator != null) services.Remove(defaultRoleValidator);
        services.AddScoped<IRoleValidator<ApplicationRole>, TenantAwareRoleValidator>();

        services.AddScoped<ITokenService, JwtService>();
        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<ILoginLogService, LoginLogService>();
        services.AddScoped<IClientIpService, ClientIpService>();
        services.AddScoped<IGeoIpService, GeoIpService>();
        services.AddScoped<IUserAgentParserService, UserAgentParserService>();
        services.AddScoped<IPermissionService, PermissionService>();
        services.AddScoped<IAuditLogPersister, AuditLogPersister>();
        services.AddScoped<IAuditService, AuditService>();
        services.AddScoped<IDataSanitizer, DataSanitizer>();
        services.AddScoped<SuperAdminSeeder>();

        services.AddHangfire(config => config
            .UsePostgreSqlStorage(c =>
                c.UseNpgsqlConnection(configuration.GetConnectionString("PostgresSqlConnection")))
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings());

        services.AddHangfireServer(options => { options.WorkerCount = Environment.ProcessorCount * 2; });
        return services;
    }
}