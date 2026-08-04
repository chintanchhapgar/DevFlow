using DevFlow.Notification.Application.Common.Abstractions.Persistence;
using DevFlow.Notification.Domain.Notifications;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Notification.Application.Notifications.MarkAsRead;

internal sealed class MarkNotificationAsReadCommandHandler
    : IRequestHandler<
        MarkNotificationAsReadCommand,
        Result<MarkNotificationAsReadResponse>>
{
    private readonly INotificationRepository _notificationRepository;
    private readonly IUnitOfWork _unitOfWork;

    public MarkNotificationAsReadCommandHandler(
        INotificationRepository notificationRepository,
        IUnitOfWork unitOfWork)
    {
        _notificationRepository = notificationRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<MarkNotificationAsReadResponse>> Handle(
    MarkNotificationAsReadCommand request,
    CancellationToken cancellationToken)
    {
        var notification =
            await _notificationRepository.GetByIdAsync(
                new NotificationId(request.NotificationId),
                cancellationToken);

        if (notification is null)
        {
            return Result.Failure<MarkNotificationAsReadResponse>(
                NotificationErrors.NotFound);
        }

        var result = notification.MarkAsRead();

        if (result.IsFailure)
        {
            return Result.Failure<MarkNotificationAsReadResponse>(
                result.Error);
        }

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return new MarkNotificationAsReadResponse(
            notification.Id.Value,
            notification.ReadOnUtc!.Value);
    }
}
