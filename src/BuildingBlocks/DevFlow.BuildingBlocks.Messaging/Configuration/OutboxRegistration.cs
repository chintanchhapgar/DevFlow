using DevFlow.BuildingBlocks.Infrastructure.Outbox;
using DevFlow.BuildingBlocks.Messaging.IntegrationEvents;
using DevFlow.BuildingBlocks.Messaging.Outbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DevFlow.BuildingBlocks.Messaging;

internal static class OutboxRegistration
{
    internal static IServiceCollection AddOutbox<TContext>(
        this IServiceCollection services)
        where TContext : DbContext
    {
        ArgumentNullException.ThrowIfNull(services);

        // Shared infrastructure
        services.AddSingleton<IEventTypeResolver, CachedEventTypeResolver>();

        // Service-specific registrations
        services.AddScoped<IOutboxRepository, OutboxRepository<TContext>>();

        services.AddScoped<IIntegrationEventPublisher, IntegrationEventPublisher>();

        return services;
    }
}
