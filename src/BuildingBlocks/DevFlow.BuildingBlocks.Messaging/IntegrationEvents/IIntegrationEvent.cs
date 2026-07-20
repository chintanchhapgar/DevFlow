namespace DevFlow.BuildingBlocks.Messaging.IntegrationEvents;

/// <summary>
/// Marker interface for integration events.
/// Integration events communicate state changes between services.
/// They are published after a transaction commits via the Outbox Pattern.
/// Unlike domain events, they cross service boundaries.
/// </summary>
public interface IIntegrationEvent
{
    Guid Id { get; }
    DateTime OccurredOnUtc { get; }
}
