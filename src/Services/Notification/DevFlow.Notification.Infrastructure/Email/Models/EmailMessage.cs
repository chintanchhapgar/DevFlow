namespace DevFlow.Notification.Infrastructure.Email.Models;

/// <summary>
/// Email to send.
/// </summary>
public sealed class EmailMessage
{
    public required EmailAddress To { get; init; }

    public required string Subject { get; init; }

    public required string HtmlBody { get; init; }
}
