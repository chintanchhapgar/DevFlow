using DevFlow.BuildingBlocks.Infrastructure.Outbox;
using Microsoft.EntityFrameworkCore;

namespace DevFlow.Identity.Infrastructure.Persistence.Repositories;

internal sealed class OutboxRepository : IOutboxRepository
{
    private readonly IdentityDbContext _context;

    public OutboxRepository(IdentityDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(
        OutboxMessage message,
        CancellationToken cancellationToken = default)
    {
        await _context.OutboxMessages.AddAsync(message, cancellationToken);
    }

    public async Task<List<OutboxMessage>> GetPendingMessagesAsync(
        int batchSize = 20,
        CancellationToken cancellationToken = default)
    {
        return await _context.OutboxMessages
            .Where(m => m.ProcessedOnUtc == null && m.RetryCount < 3)
            .OrderBy(m => m.CreatedOnUtc)
            .Take(batchSize)
            .ToListAsync(cancellationToken);
    }
}
