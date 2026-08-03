using DevFlow.SharedKernel.Domain.DomainEvents;

namespace DevFlow.SharedKernel.Abstractions;

public interface IEntity
{
    IReadOnlyCollection<IDomainEvent> DomainEvents { get; }

    void ClearDomainEvents();
}
