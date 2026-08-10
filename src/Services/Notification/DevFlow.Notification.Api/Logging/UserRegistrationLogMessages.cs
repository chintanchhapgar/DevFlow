using Microsoft.Extensions.Logging;

namespace DevFlow.Notification.Api.Logging;

internal static partial class UserRegistrationLogMessages
{
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

    [LoggerMessage(
     EventId = 7201,
     Level = LogLevel.Information,
     Message = "Password reset email requested for user {UserId}.")]
    public static partial void PasswordResetEmailRequested(
     this ILogger logger,
     Guid userId);
}
