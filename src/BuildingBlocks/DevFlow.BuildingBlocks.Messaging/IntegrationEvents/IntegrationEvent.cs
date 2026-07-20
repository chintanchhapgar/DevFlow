namespace DevFlow.BuildingBlocks.Messaging.IntegrationEvents;

/// <summary>
/// Base record for integration events.
/// </summary>
public abstract record IntegrationEvent : IIntegrationEvent
{
    protected IntegrationEvent()
    {
        Id = Guid.NewGuid();
        OccurredOnUtc = DateTime.UtcNow;
    }

    public Guid Id { get; init; }
    public DateTime OccurredOnUtc { get; init; }
}
