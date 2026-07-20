using DevFlow.BuildingBlocks.Messaging.IntegrationEvents;

namespace DevFlow.BuildingBlocks.Messaging.EventBus;

/// <summary>
/// Abstraction for publishing integration events.
/// Decouples application code from MassTransit specifics.
/// </summary>
public interface IEventBus
{
    Task PublishAsync<T>(
        T integrationEvent,
        CancellationToken cancellationToken = default)
        where T : class, IIntegrationEvent;
}
