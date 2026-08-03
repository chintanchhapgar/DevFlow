namespace DevFlow.SharedKernel.Domain.DomainEvents;

/// <summary>
/// Consumes a domain event.
/// </summary>
public interface IDomainEventConsumer<in TDomainEvent>
    where TDomainEvent : IDomainEvent
{
    Task ConsumeAsync(
        TDomainEvent domainEvent,
        CancellationToken cancellationToken = default);
}
