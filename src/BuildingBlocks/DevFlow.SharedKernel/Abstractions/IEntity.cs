using DevFlow.SharedKernel.Domain;

namespace DevFlow.SharedKernel.Abstractions;

public interface IEntity
{
    IReadOnlyCollection<IDomainEvent> DomainEvents { get; }

    void ClearDomainEvents();
}
