using DevFlow.BuildingBlocks.Infrastructure.DomainEvents;
using DevFlow.Identity.Application.Common.Abstractions.Persistence;
using DevFlow.SharedKernel.Abstractions;
using DevFlow.SharedKernel.Domain;
using DevFlow.SharedKernel.Domain.DomainEvents;

namespace DevFlow.Identity.Infrastructure.Persistence;

/// <summary>
/// Coordinates persistence operations and dispatches domain events.
/// </summary>
internal sealed class UnitOfWork : IUnitOfWork
{
    private readonly IdentityDbContext _context;
    private readonly IDomainEventDispatcher _domainEventDispatcher;

    public UnitOfWork(
        IdentityDbContext context,
        IDomainEventDispatcher domainEventDispatcher)
    {
        _context = context;
        _domainEventDispatcher = domainEventDispatcher;
    }

    public async Task<int> SaveChangesAsync(
    CancellationToken cancellationToken = default)
    {
        List<IDomainEvent> domainEvents = GetDomainEvents();

        int result = await _context.SaveChangesAsync(cancellationToken);

        if (domainEvents.Count > 0)
        {
            await _domainEventDispatcher.DispatchAsync(
                domainEvents,
                cancellationToken);

            // Persist any OutboxMessages added by domain event consumers
            await _context.SaveChangesAsync(cancellationToken);
        }

        return result;
    }

    private List<IDomainEvent> GetDomainEvents()
    {
        var entities = _context.ChangeTracker
            .Entries<IHasDomainEvents>()
            .Select(entry => entry.Entity)
            .Where(entity => entity.DomainEvents.Count > 0)
            .ToList();

        var domainEvents = entities
            .SelectMany(entity => entity.DomainEvents)
            .ToList();

        foreach (var entity in entities)
        {
            entity.ClearDomainEvents();
        }

        return domainEvents;
    }
}
