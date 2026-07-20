namespace DevFlow.SharedKernel.Domain;

/// <summary>
/// Base record for domain events.
/// Using record ensures structural equality and immutability.
/// </summary>
public abstract record DomainEvent : IDomainEvent
{
    protected DomainEvent()
    {
        Id = Guid.NewGuid();
        OccurredOnUtc = DateTime.UtcNow;
    }

    public Guid Id { get; init; }
    public DateTime OccurredOnUtc { get; init; }
}
