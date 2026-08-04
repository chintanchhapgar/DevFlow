using DevFlow.SharedKernel.Results;

namespace DevFlow.Notification.Domain.Notifications;

public static class NotificationErrors
{
    public static readonly AppError NotFound =
        AppError.NotFound(
            "Notifications.NotFound",
            "Notification was not found.");

    public static readonly AppError AlreadyRead =
        AppError.Conflict(
            "Notifications.AlreadyRead",
            "Notification has already been read.");
}
