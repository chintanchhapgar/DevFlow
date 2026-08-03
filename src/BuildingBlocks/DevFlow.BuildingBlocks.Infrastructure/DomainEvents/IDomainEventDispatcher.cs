using DevFlow.SharedKernel.Domain.DomainEvents;

namespace DevFlow.BuildingBlocks.Infrastructure.DomainEvents;

public interface IDomainEventDispatcher
{
    Task DispatchAsync(
        IReadOnlyCollection<IDomainEvent> domainEvents,
        CancellationToken cancellationToken = default);
}
