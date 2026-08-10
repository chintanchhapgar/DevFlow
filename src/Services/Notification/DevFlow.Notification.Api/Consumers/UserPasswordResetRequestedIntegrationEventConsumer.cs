using DevFlow.BuildingBlocks.Contracts.IntegrationEvents.Identity;
using DevFlow.Notification.Api.Logging;
using DevFlow.Notification.Infrastructure.Email.PasswordReset;
using MassTransit;

namespace DevFlow.Notification.Api.Consumers;

public sealed class UserPasswordResetRequestedIntegrationEventConsumer
    : IConsumer<UserPasswordResetRequestedIntegrationEvent>
{
    private readonly PasswordResetEmailSender _passwordResetEmailSender;
    private readonly ILogger<UserPasswordResetRequestedIntegrationEventConsumer> _logger;

    public UserPasswordResetRequestedIntegrationEventConsumer(
        PasswordResetEmailSender passwordResetEmailSender,
        ILogger<UserPasswordResetRequestedIntegrationEventConsumer> logger)
    {
        _passwordResetEmailSender = passwordResetEmailSender;
        _logger = logger;
    }

    public async Task Consume(
        ConsumeContext<UserPasswordResetRequestedIntegrationEvent> context)
    {
        var message = context.Message;

        _logger.PasswordResetEmailRequested(message.UserId);

        await _passwordResetEmailSender.SendAsync(
            message.Email,
            message.FirstName,
            message.ResetToken,
            context.CancellationToken);
    }
}
