using Microsoft.Extensions.Logging;

namespace DevFlow.Notification.Infrastructure.Email.PasswordReset;

internal static partial class PasswordResetEmailLogMessages
{
    [LoggerMessage(
        EventId = 7101,
        Level = LogLevel.Information,
        Message = "Password reset email sent to {Email}.")]
    public static partial void PasswordResetEmailSent(
        this ILogger logger,
        string email);
}
