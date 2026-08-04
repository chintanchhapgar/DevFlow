using DevFlow.Notification.Application.Common.Abstractions.Persistence;
using DevFlow.Notification.Domain.Notifications;
using Microsoft.EntityFrameworkCore;

namespace DevFlow.Notification.Infrastructure.Persistence.Repositories;

internal sealed class NotificationRepository
    : INotificationRepository
{
    private readonly NotificationDbContext _context;

    public NotificationRepository(
        NotificationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(
        DevFlow.Notification.Domain.Notifications.Notification notification,
        CancellationToken cancellationToken = default)
    {
        await _context.Notifications.AddAsync(
            notification,
            cancellationToken);
    }

    public async Task<DevFlow.Notification.Domain.Notifications.Notification?> GetByIdAsync(
        NotificationId id,
        CancellationToken cancellationToken = default)
    {
        return await _context.Notifications
            .FirstOrDefaultAsync(
                x => x.Id == id,
                cancellationToken);
    }

    public async Task<List<DevFlow.Notification.Domain.Notifications.Notification>> GetByUserAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        return await _context.Notifications
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.CreatedOnUtc)
            .ToListAsync(cancellationToken);
    }
}
