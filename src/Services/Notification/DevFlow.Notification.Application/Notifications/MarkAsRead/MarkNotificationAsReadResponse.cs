namespace DevFlow.Notification.Application.Notifications.MarkAsRead;

public sealed record MarkNotificationAsReadResponse(
    Guid NotificationId,
    DateTime ReadOnUtc);
