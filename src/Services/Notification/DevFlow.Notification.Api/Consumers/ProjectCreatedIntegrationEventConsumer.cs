using DevFlow.BuildingBlocks.Contracts.IntegrationEvents.Projects;
using DevFlow.BuildingBlocks.Messaging.Logging;
using DevFlow.Notification.Application.Common.Abstractions.Persistence;
using DevFlow.Notification.Domain.Notifications;
using DevFlow.Notification.Infrastructure.Persistence;
using MassTransit;

namespace DevFlow.Notification.Api.Consumers;

public sealed class ProjectCreatedIntegrationEventConsumer
    : IConsumer<ProjectCreatedIntegrationEvent>
{
    private readonly INotificationRepository _notificationRepository;
    private readonly NotificationDbContext _dbContext;
    private readonly ILogger<ProjectCreatedIntegrationEventConsumer> _logger;

    public ProjectCreatedIntegrationEventConsumer(
        INotificationRepository notificationRepository,
        NotificationDbContext dbContext,
        ILogger<ProjectCreatedIntegrationEventConsumer> logger)
    {
        _notificationRepository = notificationRepository;
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task Consume(
        ConsumeContext<ProjectCreatedIntegrationEvent> context)
    {
        var message = context.Message;
        Console.WriteLine("=== Notification received ProjectCreatedIntegrationEvent ===");
        _logger.LogIntegrationEventReceived(
            nameof(ProjectCreatedIntegrationEvent));

        var notification =
            DevFlow.Notification.Domain.Notifications.Notification.Create(
                message.OwnerId,
                "Project Created",
                $"Project '{message.Name}' was created successfully.",
                NotificationType.Project);

        await _notificationRepository.AddAsync(notification);

        await _dbContext.SaveChangesAsync(
            context.CancellationToken);

        _logger.LogProjectCreated(message.Name);
    }
}
