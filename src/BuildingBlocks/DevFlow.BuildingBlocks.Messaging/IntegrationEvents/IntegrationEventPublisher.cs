using DevFlow.BuildingBlocks.Infrastructure.Outbox;
using DevFlow.BuildingBlocks.Messaging.Outbox;
using DevFlow.BuildingBlocks.Messaging.Serialization;

namespace DevFlow.BuildingBlocks.Messaging.IntegrationEvents;

/// <summary>
/// Stores integration events in the transactional outbox.
/// </summary>
internal sealed class IntegrationEventPublisher
    : IIntegrationEventPublisher
{
    private readonly IOutboxRepository _outboxRepository;
    private readonly IMessageSerializer _serializer;

    public IntegrationEventPublisher(
    IOutboxRepository outboxRepository,
    IMessageSerializer serializer)
    {
        ArgumentNullException.ThrowIfNull(outboxRepository);
        ArgumentNullException.ThrowIfNull(serializer);

        _outboxRepository = outboxRepository;
        _serializer = serializer;
    }

    public async Task PublishAsync(
        IIntegrationEvent integrationEvent,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(integrationEvent);

        var eventType = integrationEvent.GetType();

        var typeName = eventType.AssemblyQualifiedName
            ?? throw new InvalidOperationException(
                $"Assembly-qualified name not found for '{eventType.FullName}'.");

        Console.WriteLine("=== IntegrationEventPublisher.PublishAsync ===");
        Console.WriteLine(new System.Diagnostics.StackTrace(true).ToString());

        var message = OutboxMessage.Create(
            typeName,
            _serializer.Serialize(integrationEvent),
            integrationEvent.OccurredOnUtc);

        

        await _outboxRepository.AddAsync(
            message,
            cancellationToken);

        Console.WriteLine("=== Outbox message added ===");
    }
}
