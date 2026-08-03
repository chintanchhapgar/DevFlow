namespace DevFlow.BuildingBlocks.Infrastructure.Outbox;

/// <summary>
/// Repository for transactional outbox messages.
/// </summary>
public interface IOutboxRepository
{
    Task AddAsync(
        OutboxMessage message,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<OutboxMessage>> GetPendingMessagesAsync(
        int batchSize,
        CancellationToken cancellationToken = default);
}
