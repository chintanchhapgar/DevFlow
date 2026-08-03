using DevFlow.SharedKernel.Domain.DomainEvents;

namespace DevFlow.SharedKernel.Domain;

/// <summary>
/// Represents an entity that can raise domain events.
/// </summary>
public interface IHasDomainEvents
{
    IReadOnlyCollection<IDomainEvent> DomainEvents { get; }

    void ClearDomainEvents();
}
