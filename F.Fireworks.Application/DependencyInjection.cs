using System.Reflection;
using F.Fireworks.Application.Common.Behaviors;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace F.Fireworks.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // 注册 MediatR
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        // 注册 FluentValidation 验证器
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        // 注册 MediatR 验证管道行为
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        return services;
    }
}