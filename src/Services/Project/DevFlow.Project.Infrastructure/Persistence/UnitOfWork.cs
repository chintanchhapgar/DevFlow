using DevFlow.BuildingBlocks.Infrastructure.DomainEvents;
using DevFlow.Identity.Application.Common.Abstractions.Persistence;
using DevFlow.SharedKernel.Abstractions;
using DevFlow.SharedKernel.Domain;
using DevFlow.SharedKernel.Domain.DomainEvents;

namespace DevFlow.Project.Infrastructure.Persistence;

internal sealed class UnitOfWork : IUnitOfWork
{
    private readonly ProjectDbContext _context;
    private readonly IDomainEventDispatcher _domainEventDispatcher;

    public UnitOfWork(
        ProjectDbContext context,
        IDomainEventDispatcher domainEventDispatcher)
    {
        _context = context;
        _domainEventDispatcher = domainEventDispatcher;
    }

    public async Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        var domainEvents = GetDomainEvents();

        var result = await _context.SaveChangesAsync(cancellationToken);

        if (domainEvents.Count > 0)
        {
            await _domainEventDispatcher.DispatchAsync(
                domainEvents,
                cancellationToken);

            // Save OutboxMessages created by domain event consumers
            await _context.SaveChangesAsync(cancellationToken);
        }

        return result;
    }

    private List<IDomainEvent> GetDomainEvents()
    {
        var entities = _context.ChangeTracker
            .Entries<IHasDomainEvents>()
            .Select(x => x.Entity)
            .Where(x => x.DomainEvents.Count > 0)
            .ToList();

        var domainEvents = entities
            .SelectMany(x => x.DomainEvents)
            .ToList();

        foreach (var entity in entities)
        {
            entity.ClearDomainEvents();
        }

        return domainEvents;
    }
}
