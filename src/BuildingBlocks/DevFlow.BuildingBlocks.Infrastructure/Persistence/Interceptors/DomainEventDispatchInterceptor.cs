using DevFlow.BuildingBlocks.Infrastructure.DomainEvents;
using DevFlow.SharedKernel.Abstractions;
using DevFlow.SharedKernel.Domain;
using DevFlow.SharedKernel.Domain.DomainEvents;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace DevFlow.BuildingBlocks.Infrastructure.Persistence.Interceptors;

public sealed class DomainEventDispatchInterceptor
    : SaveChangesInterceptor
{
    private readonly IDomainEventDispatcher _dispatcher;

    public DomainEventDispatchInterceptor(
        IDomainEventDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
    }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        if (eventData.Context is not null)
        {
            await DispatchDomainEventsAsync(
                eventData.Context,
                cancellationToken);
        }

        return await base.SavingChangesAsync(
            eventData,
            result,
            cancellationToken);
    }

    private async Task DispatchDomainEventsAsync(
        DbContext context,
        CancellationToken cancellationToken)
    {
        var entities = context.ChangeTracker
            .Entries<IHasDomainEvents>()
            .Select(x => x.Entity)
            .Where(x => x.DomainEvents.Count > 0)
            .ToList();

        if (entities.Count == 0)
        {
            return;
        }

        var domainEvents = entities
            .SelectMany(x => x.DomainEvents)
            .OrderBy(x => (x as DomainEvent)?.OccurredOnUtc ?? DateTime.MinValue)
            .ToList();

        foreach (var entity in entities)
        {
            entity.ClearDomainEvents();
        }
        Console.WriteLine($"Dispatching {domainEvents.Count} domain event(s)");

        foreach (var e in domainEvents)
        {
            Console.WriteLine(e.GetType().Name);
        }
        await _dispatcher.DispatchAsync(
            domainEvents,
            cancellationToken);
    }
}
