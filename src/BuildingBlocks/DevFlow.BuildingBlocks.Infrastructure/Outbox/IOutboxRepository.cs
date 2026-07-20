namespace DevFlow.BuildingBlocks.Infrastructure.Outbox;

/// <summary>
/// Contract for outbox message persistence operations.
/// </summary>
public interface IOutboxRepository
{
    Task AddAsync(OutboxMessage message, CancellationToken cancellationToken = default);

    Task<List<OutboxMessage>> GetPendingMessagesAsync(
        int batchSize = 20,
        CancellationToken cancellationToken = default);
}
