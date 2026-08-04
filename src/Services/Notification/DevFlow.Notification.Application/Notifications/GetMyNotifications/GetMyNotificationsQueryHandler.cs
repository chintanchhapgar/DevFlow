using DevFlow.Notification.Application.Common.Abstractions.Persistence;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Notification.Application.Notifications.GetMyNotifications;

internal sealed class GetMyNotificationsQueryHandler
    : IRequestHandler<
        GetMyNotificationsQuery,
        Result<List<NotificationResponse>>>
{
    private readonly INotificationRepository _notificationRepository;

    public GetMyNotificationsQueryHandler(
        INotificationRepository notificationRepository)
    {
        _notificationRepository = notificationRepository;
    }

    public async Task<Result<List<NotificationResponse>>> Handle(
        GetMyNotificationsQuery request,
        CancellationToken cancellationToken)
    {
        var notifications =
            await _notificationRepository.GetByUserAsync(
                request.UserId,
                cancellationToken);

        var response = notifications
            .Select(notification =>
                new NotificationResponse(
                    notification.Id.Value,
                    notification.Title,
                    notification.Message,
                    (int)notification.Type,
                    (int)notification.Status,
                    notification.CreatedOnUtc,
                    notification.ReadOnUtc))
            .ToList();

        return Result.Success(response);
    }
}
