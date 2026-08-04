using Microsoft.Extensions.DependencyInjection;

namespace DevFlow.BuildingBlocks.Messaging.Outbox;

internal static class OutboxRegistration
{
    internal static IServiceCollection AddOutbox(
        this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddSingleton<IIntegrationEventTypeResolver, CachedIntegrationEventTypeResolver>();

        return services;
    }
}
