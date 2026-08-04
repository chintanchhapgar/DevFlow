namespace DevFlow.Notification.Application.Notifications.GetMyNotifications;

public sealed record NotificationResponse(
    Guid Id,
    string Title,
    string Message,
    int Type,
    int Status,
    DateTime CreatedOnUtc,
    DateTime? ReadOnUtc);
