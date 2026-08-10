using DevFlow.Notification.Infrastructure.Email.Models;

namespace DevFlow.Notification.Infrastructure.Email.Sending;

/// <summary>
/// Sends emails through the configured email provider.
/// </summary>
public interface IEmailSender
{
    Task SendAsync(
        EmailMessage message,
        CancellationToken cancellationToken = default);
}
