using Microsoft.Extensions.Logging;

namespace DevFlow.Notification.Infrastructure.Email.Verification;

internal static partial class VerificationEmailLogMessages
{
    [LoggerMessage(
        EventId = 7101,
        Level = LogLevel.Information,
        Message = "Sending email verification email to {Email}")]
    public static partial void SendingVerificationEmail(
        this ILogger logger,
        string email);

    [LoggerMessage(
        EventId = 7102,
        Level = LogLevel.Information,
        Message = "Email verification email sent to {Email}")]
    public static partial void VerificationEmailSent(
        this ILogger logger,
        string email);

    [LoggerMessage(
        EventId = 7103,
        Level = LogLevel.Information,
        Message = "Received UserRegisteredIntegrationEvent for user {UserId}")]
    public static partial void UserRegisteredEventReceived(
        this ILogger logger,
        Guid userId);

    [LoggerMessage(
        EventId = 7104,
        Level = LogLevel.Information,
        Message = "Received UserEmailVerificationResentIntegrationEvent for user {UserId}")]
    public static partial void VerificationResentEventReceived(
        this ILogger logger,
        Guid userId);
}
