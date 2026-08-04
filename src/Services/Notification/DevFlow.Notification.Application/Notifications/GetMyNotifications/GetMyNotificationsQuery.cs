using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Notification.Application.Notifications.GetMyNotifications;

public sealed record GetMyNotificationsQuery(
    Guid UserId)
    : IRequest<Result<List<NotificationResponse>>>;
