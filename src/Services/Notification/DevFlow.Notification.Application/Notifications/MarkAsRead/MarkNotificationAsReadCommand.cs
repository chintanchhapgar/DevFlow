using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Notification.Application.Notifications.MarkAsRead;

public sealed record MarkNotificationAsReadCommand(
    Guid NotificationId)
    : IRequest<Result<MarkNotificationAsReadResponse>>;
