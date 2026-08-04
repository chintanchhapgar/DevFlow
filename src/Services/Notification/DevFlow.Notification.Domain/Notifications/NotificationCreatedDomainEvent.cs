using DevFlow.SharedKernel.Domain.DomainEvents;

namespace DevFlow.Notification.Domain.Notifications;

public sealed record NotificationCreatedDomainEvent(
    NotificationId NotificationId,
    Guid UserId)
    : DomainEvent;
