using DevFlow.BuildingBlocks.Messaging.IntegrationEvents;
using MassTransit;

namespace DevFlow.BuildingBlocks.Messaging.EventBus;

/// <summary>
/// MassTransit implementation of IEventBus.
/// Publishes messages to the configured transport (RabbitMQ).
/// </summary>
public sealed class MassTransitEventBus : IEventBus
{
    private readonly IPublishEndpoint _publishEndpoint;

    public MassTransitEventBus(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task PublishAsync<T>(
        T integrationEvent,
        CancellationToken cancellationToken = default)
        where T : class, IIntegrationEvent
    {
        await _publishEndpoint.Publish(integrationEvent, cancellationToken);
    }
}
