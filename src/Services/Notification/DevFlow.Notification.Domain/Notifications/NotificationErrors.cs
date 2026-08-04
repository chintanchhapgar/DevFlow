using DevFlow.SharedKernel.Results;

namespace DevFlow.Notification.Domain.Notifications;

public static class NotificationErrors
{
    public static readonly AppError AlreadyRead =
        AppError.Conflict(
            "Notification.AlreadyRead",
            "Notification has already been read.");
}
