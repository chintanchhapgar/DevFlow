using DevFlow.Notification.Domain.Notifications;

namespace DevFlow.Notification.Application.Common.Abstractions.Persistence;

public interface INotificationRepository
{
    Task AddAsync(
        DevFlow.Notification.Domain.Notifications.Notification notification,
        CancellationToken cancellationToken = default);

    Task<DevFlow.Notification.Domain.Notifications.Notification?> GetByIdAsync(
        NotificationId id,
        CancellationToken cancellationToken = default);

    Task<List<DevFlow.Notification.Domain.Notifications.Notification>> GetByUserAsync(
        Guid userId,
        CancellationToken cancellationToken = default);
}
