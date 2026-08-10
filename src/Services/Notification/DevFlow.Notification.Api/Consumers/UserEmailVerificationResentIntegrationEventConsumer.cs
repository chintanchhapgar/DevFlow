using DevFlow.BuildingBlocks.Contracts.IntegrationEvents.Users;
using DevFlow.Notification.Api.Logging;
using DevFlow.Notification.Infrastructure.Email.Verification;
using MassTransit;

namespace DevFlow.Notification.Api.Consumers;

public sealed class UserEmailVerificationResentIntegrationEventConsumer
    : IConsumer<UserEmailVerificationResentIntegrationEvent>
{
    private readonly VerificationEmailSender _verificationEmailSender;
    private readonly ILogger<UserEmailVerificationResentIntegrationEventConsumer> _logger;

    public UserEmailVerificationResentIntegrationEventConsumer(
        VerificationEmailSender verificationEmailSender,
        ILogger<UserEmailVerificationResentIntegrationEventConsumer> logger)
    {
        _verificationEmailSender = verificationEmailSender;
        _logger = logger;
    }

    public async Task Consume(
        ConsumeContext<UserEmailVerificationResentIntegrationEvent> context)
    {
        var message = context.Message;

        _logger.VerificationResentEventReceived(
            message.UserId);

        await _verificationEmailSender.SendAsync(
            message.Email,
            message.FirstName,
            message.VerificationToken,
            context.CancellationToken);
    }
}
