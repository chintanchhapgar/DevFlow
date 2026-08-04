using DevFlow.BuildingBlocks.Contracts.IntegrationEvents.Identity;
using DevFlow.BuildingBlocks.Messaging.Logging;
using MassTransit;

namespace DevFlow.Notification.Api.Consumers;

public sealed class UserRegisteredIntegrationEventConsumer
    : IConsumer<UserRegisteredIntegrationEvent>
{
    private readonly ILogger<UserRegisteredIntegrationEventConsumer> _logger;

    public UserRegisteredIntegrationEventConsumer(
        ILogger<UserRegisteredIntegrationEventConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(
        ConsumeContext<UserRegisteredIntegrationEvent> context)
    {
        _logger.LogIntegrationEventReceived(
            nameof(UserRegisteredIntegrationEvent));

        return Task.CompletedTask;
    }
}
