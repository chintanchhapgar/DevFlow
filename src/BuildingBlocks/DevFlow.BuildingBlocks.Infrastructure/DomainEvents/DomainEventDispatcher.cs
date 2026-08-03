using System.Collections.Concurrent;
using System.Linq.Expressions;
using DevFlow.SharedKernel.Domain.DomainEvents;
using Microsoft.Extensions.DependencyInjection;

namespace DevFlow.BuildingBlocks.Infrastructure.DomainEvents;

/// <summary>
/// Dispatches domain events to all registered consumers.
/// Reflection is performed only once per domain event type.
/// </summary>
public sealed class DomainEventDispatcher : IDomainEventDispatcher
{
    private static readonly ConcurrentDictionary<Type, Func<object, IDomainEvent, CancellationToken, Task>>
        ConsumerInvokers = new();

    private readonly IServiceProvider _serviceProvider;

    public DomainEventDispatcher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task DispatchAsync(
        IReadOnlyCollection<IDomainEvent> domainEvents,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(domainEvents);

        foreach (var domainEvent in domainEvents)
        {
            var consumerType = typeof(IDomainEventConsumer<>)
                .MakeGenericType(domainEvent.GetType());

            var consumers = _serviceProvider.GetServices(consumerType);

            var invoker = ConsumerInvokers.GetOrAdd(
                consumerType,
                CreateInvoker);

            foreach (var consumer in consumers)
            {
                if (consumer is null)
                {
                    continue;
                }

                await invoker(
                    consumer,
                    domainEvent,
                    cancellationToken);
            }
        }
    }

    private static Func<object, IDomainEvent, CancellationToken, Task> CreateInvoker(
        Type consumerType)
    {
        var consumer = Expression.Parameter(typeof(object));
        var domainEvent = Expression.Parameter(typeof(IDomainEvent));
        var cancellationToken = Expression.Parameter(typeof(CancellationToken));

        var consumeMethod = consumerType.GetMethod(nameof(IDomainEventConsumer<IDomainEvent>.ConsumeAsync))
            ?? throw new InvalidOperationException(
                $"ConsumeAsync method not found for {consumerType.Name}.");

        var body = Expression.Call(
            Expression.Convert(consumer, consumerType),
            consumeMethod,
            Expression.Convert(domainEvent, consumeMethod.GetParameters()[0].ParameterType),
            cancellationToken);

        return Expression.Lambda<Func<object, IDomainEvent, CancellationToken, Task>>(
            body,
            consumer,
            domainEvent,
            cancellationToken)
            .Compile();
    }
}
