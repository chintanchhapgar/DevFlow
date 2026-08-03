using DevFlow.BuildingBlocks.Infrastructure.DomainEvents;
using DevFlow.SharedKernel.Abstractions;
using DevFlow.SharedKernel.Domain;
using DevFlow.SharedKernel.Domain.DomainEvents;
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
    private readonly IDomainEventDispatcher _dispatcher;

    public DomainEventDispatchInterceptor(
     IDomainEventDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
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
        var entities = context.ChangeTracker
        .Entries<IHasDomainEvents>()
        .Select(entry => entry.Entity)
        .Where(entity => entity.DomainEvents.Count > 0)
        .ToList();

            var domainEvents = entities
                .SelectMany(entity => entity.DomainEvents)
                .OrderBy(e => (e as DomainEvent)?.OccurredOnUtc ?? DateTime.MinValue)
                .ToList();

            entities.ForEach(entity => entity.ClearDomainEvents());

        await _dispatcher.DispatchAsync(
            domainEvents,
            cancellationToken);
    }
}
