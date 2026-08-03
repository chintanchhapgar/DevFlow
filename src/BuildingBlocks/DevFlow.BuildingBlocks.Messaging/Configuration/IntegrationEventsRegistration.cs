using DevFlow.BuildingBlocks.Messaging.IntegrationEvents;
using Microsoft.Extensions.DependencyInjection;

namespace DevFlow.BuildingBlocks.Messaging;

internal static class IntegrationEventsRegistration
{
    internal static IServiceCollection AddIntegrationEvents(
    this IServiceCollection services)
    {
        return services;
    }
}
