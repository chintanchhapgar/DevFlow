using DevFlow.SharedKernel.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace DevFlow.BuildingBlocks.Infrastructure.Persistence.Interceptors;

/// <summary>
/// EF Core SaveChanges interceptor that dispatches domain events after persistence.
/// Events are dispatched AFTER the transaction commits to ensure consistency.
/// </summary>
public sealed class DomainEventDispatchInterceptor : SaveChangesInterceptor
{
    private readonly IPublisher _publisher;

    public DomainEventDispatchInterceptor(IPublisher publisher)
    {
        _publisher = publisher;
    }

    public override async ValueTask<int> SavedChangesAsync(
        SaveChangesCompletedEventData eventData,
        int result,
        CancellationToken cancellationToken = default)
    {
        if (eventData.Context is not null)
        {
            await DispatchDomainEventsAsync(eventData.Context, cancellationToken);
        }

        return await base.SavedChangesAsync(eventData, result, cancellationToken);
    }

    private async Task DispatchDomainEventsAsync(
        DbContext context,
        CancellationToken cancellationToken)
    {
        var aggregateRoots = context.ChangeTracker
            .Entries<IAggregateRootMarker>()
            .Select(e => e.Entity)
            .Where(ar => ar.DomainEvents.Count != 0)
            .ToList();

        var domainEvents = aggregateRoots
            .SelectMany(ar => ar.DomainEvents)
            .ToList();

        // Clear events before dispatching to prevent re-dispatch
        aggregateRoots.ForEach(ar => ar.ClearDomainEvents());

        foreach (var domainEvent in domainEvents)
        {
            await _publisher.Publish(domainEvent, cancellationToken);
        }
    }
}

/// <summary>
/// Internal marker interface for EF Core change tracker access.
/// </summary>
internal interface IAggregateRootMarker
{
    IReadOnlyCollection<IDomainEvent> DomainEvents { get; }
    void ClearDomainEvents();
}
