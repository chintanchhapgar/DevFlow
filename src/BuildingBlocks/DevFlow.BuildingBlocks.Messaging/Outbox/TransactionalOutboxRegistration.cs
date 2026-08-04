using DevFlow.BuildingBlocks.Infrastructure.Outbox;
using DevFlow.BuildingBlocks.Messaging.IntegrationEvents;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DevFlow.BuildingBlocks.Messaging.Outbox;

/// <summary>
/// Registers transactional outbox services.
/// Only services that publish integration events should use this.
/// </summary>
public static class TransactionalOutboxRegistration
{
    public static IServiceCollection AddTransactionalOutbox<TContext>(
        this IServiceCollection services)
        where TContext : DbContext
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddScoped<IOutboxRepository,
            OutboxRepository<TContext>>();

        services.AddScoped<IIntegrationEventPublisher,
            IntegrationEventPublisher>();

        return services;
    }
}
