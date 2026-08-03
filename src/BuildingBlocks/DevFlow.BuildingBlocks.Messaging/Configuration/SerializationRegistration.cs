using DevFlow.BuildingBlocks.Messaging.Serialization;
using Microsoft.Extensions.DependencyInjection;

namespace DevFlow.BuildingBlocks.Messaging;

internal static class SerializationRegistration
{
    internal static IServiceCollection AddSerialization(
        this IServiceCollection services)
    {
        services.AddSingleton<IMessageSerializer, JsonMessageSerializer>();

        return services;
    }
}
