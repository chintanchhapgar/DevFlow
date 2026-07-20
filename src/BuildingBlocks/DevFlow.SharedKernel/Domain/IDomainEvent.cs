namespace DevFlow.SharedKernel.Domain;

/// <summary>
/// Marker interface for domain events.
/// Domain events represent something that happened in the domain.
/// They are raised by aggregate roots and handled within the same transaction.
/// </summary>
public interface IDomainEvent
{
    /// <summary>
    /// Unique identifier for this event occurrence.
    /// </summary>
    Guid Id { get; }

    /// <summary>
    /// The UTC timestamp when this event occurred.
    /// </summary>
    DateTime OccurredOnUtc { get; }
}
