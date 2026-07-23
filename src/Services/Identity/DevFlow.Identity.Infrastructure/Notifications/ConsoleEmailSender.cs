using DevFlow.Identity.Application.Common.Abstractions.Notifications;

namespace DevFlow.Identity.Infrastructure.Notifications;

internal sealed class ConsoleEmailSender
    : IEmailSender
{
    public Task SendPasswordResetEmailAsync(
        string email,
        string resetToken,
        CancellationToken cancellationToken = default)
    {
        Console.WriteLine("======================================");
        Console.WriteLine("PASSWORD RESET EMAIL");
        Console.WriteLine($"Email : {email}");
        Console.WriteLine($"Token : {resetToken}");
        Console.WriteLine("======================================");

        return Task.CompletedTask;
    }
}
