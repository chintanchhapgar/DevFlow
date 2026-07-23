namespace DevFlow.Identity.Application.Common.Abstractions.Notifications;

public interface IEmailSender
{
    Task SendPasswordResetEmailAsync(
        string email,
        string resetToken,
        CancellationToken cancellationToken = default);
}
