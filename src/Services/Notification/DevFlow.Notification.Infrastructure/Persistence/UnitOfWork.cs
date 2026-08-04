using DevFlow.Notification.Application.Common.Abstractions.Persistence;

namespace DevFlow.Notification.Infrastructure.Persistence;

internal sealed class UnitOfWork : IUnitOfWork
{
    private readonly NotificationDbContext _context;

    public UnitOfWork(NotificationDbContext context)
    {
        _context = context;
    }

    public Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }
}
