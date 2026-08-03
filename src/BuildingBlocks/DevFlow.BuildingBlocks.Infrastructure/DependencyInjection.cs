using DevFlow.BuildingBlocks.Infrastructure.DomainEvents;
using DevFlow.BuildingBlocks.Infrastructure.Outbox;
using DevFlow.BuildingBlocks.Infrastructure.Persistence.Interceptors;
using DevFlow.BuildingBlocks.Infrastructure.Services;
using DevFlow.SharedKernel.Abstractions;
using DevFlow.SharedKernel.Common;
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

        // Interceptors are registered as singletons in EF Core
        services.AddSingleton<AuditableInterceptor>();
        services.AddSingleton<SoftDeleteInterceptor>();
        services.AddScoped<DomainEventDispatchInterceptor>();
        services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();
        services.AddScoped<DomainEventDispatchInterceptor>();

        return services;
    }
}
