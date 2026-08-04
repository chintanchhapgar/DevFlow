using DevFlow.SharedKernel.Domain;
using DevFlow.SharedKernel.Results;

namespace DevFlow.Notification.Domain.Notifications;

public sealed class Notification
    : AggregateRoot<NotificationId>
{
    private Notification(
        NotificationId id,
        Guid userId,
        string title,
        string message,
        NotificationType type)
        : base(id)
    {
        UserId = userId;
        Title = title;
        Message = message;
        Type = type;

        Status = NotificationStatus.Unread;

        CreatedOnUtc = DateTime.UtcNow;

        RaiseDomainEvent(
            new NotificationCreatedDomainEvent(
                id,
                userId));
    }

    private Notification()
    {
    }

    public Guid UserId { get; private set; }

    public string Title { get; private set; } = string.Empty;

    public string Message { get; private set; } = string.Empty;

    public NotificationType Type { get; private set; }

    public NotificationStatus Status { get; private set; }

    public DateTime CreatedOnUtc { get; private set; }

    public DateTime? ReadOnUtc { get; private set; }

    public bool IsRead =>
        Status == NotificationStatus.Read;

    public static Notification Create(
        Guid userId,
        string title,
        string message,
        NotificationType type)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(title);
        ArgumentException.ThrowIfNullOrWhiteSpace(message);

        return new Notification(
            NotificationId.New(),
            userId,
            title.Trim(),
            message.Trim(),
            type);
    }

    public Result MarkAsRead()
    {
        if (IsRead)
        {
            return Result.Failure(
                NotificationErrors.AlreadyRead);
        }

        Status = NotificationStatus.Read;
        ReadOnUtc = DateTime.UtcNow;

        return Result.Success();
    }
}
