using DevFlow.BuildingBlocks.Contracts.IntegrationEvents.Identity;
using DevFlow.BuildingBlocks.Messaging.Logging;
using DevFlow.Notification.Application.Common.Abstractions.Persistence;
using DevFlow.Notification.Domain.Notifications;
using MassTransit;

namespace DevFlow.Notification.Api.Consumers;

public sealed class UserRegisteredIntegrationEventConsumer
    : IConsumer<UserRegisteredIntegrationEvent>
{
    private readonly INotificationRepository _notificationRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UserRegisteredIntegrationEventConsumer> _logger;

    public UserRegisteredIntegrationEventConsumer(
        INotificationRepository notificationRepository,
        IUnitOfWork unitOfWork,
        ILogger<UserRegisteredIntegrationEventConsumer> logger)
    {
        _notificationRepository = notificationRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task Consume(
        ConsumeContext<UserRegisteredIntegrationEvent> context)
    {
        var message = context.Message;

        _logger.LogIntegrationEventReceived(
            nameof(UserRegisteredIntegrationEvent));

        _logger.LogUserRegistered(
            message.Email);

        var notification =
            DevFlow.Notification.Domain.Notifications.Notification.Create(
                message.UserId,
                "Welcome to DevFlow!",
                $"Hi {message.FirstName}, your account has been created successfully.",
                NotificationType.System);

        await _notificationRepository.AddAsync(notification);

        await _unitOfWork.SaveChangesAsync();
    }
}
