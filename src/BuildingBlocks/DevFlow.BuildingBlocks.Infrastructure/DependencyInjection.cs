using DevFlow.BuildingBlocks.Infrastructure.Persistence.Interceptors;
using DevFlow.BuildingBlocks.Infrastructure.Services;
using DevFlow.SharedKernel.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace DevFlow.BuildingBlocks.Infrastructure;

/// <summary>
/// Infrastructure building block DI registration.
/// Each service's Infrastructure project calls this in its own DependencyInjection.cs.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services)
    {
        services.AddHttpContextAccessor();

        services.AddSingleton<IClock, SystemClock>();

        services.AddScoped<ICurrentUser, CurrentUserService>();

        // Interceptors are registered as singletons in EF Core
        services.AddSingleton<AuditableInterceptor>();
        services.AddSingleton<SoftDeleteInterceptor>();
        services.AddScoped<DomainEventDispatchInterceptor>();

        return services;
    }
}
