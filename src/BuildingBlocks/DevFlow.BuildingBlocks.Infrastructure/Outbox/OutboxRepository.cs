using Microsoft.EntityFrameworkCore;

namespace DevFlow.BuildingBlocks.Infrastructure.Outbox;

/// <summary>
/// EF Core implementation of the transactional outbox repository.
/// </summary>
public sealed class OutboxRepository<TContext> : IOutboxRepository
    where TContext : DbContext
{
    private readonly TContext _dbContext;

    public OutboxRepository(TContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext);

        _dbContext = dbContext;
    }

    public async Task AddAsync(
        OutboxMessage message,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(message);

        await _dbContext
            .Set<OutboxMessage>()
            .AddAsync(message, cancellationToken);
    }

    public async Task<IReadOnlyList<OutboxMessage>> GetPendingMessagesAsync(
        int batchSize,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext
            .Set<OutboxMessage>()
            .Where(x => !x.IsProcessed)
            .OrderBy(x => x.CreatedOnUtc)
            .Take(batchSize)
            .ToListAsync(cancellationToken);
    }
}
