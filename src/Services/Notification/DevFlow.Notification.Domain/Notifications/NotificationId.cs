using DevFlow.SharedKernel.Domain;

namespace DevFlow.Notification.Domain.Notifications;

public sealed record NotificationId(Guid Value)
    : StronglyTypedId<Guid>(Value)
{
    public static NotificationId New() => new(Guid.NewGuid());
}
