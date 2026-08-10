namespace DevFlow.Notification.Infrastructure.Email.Models;

/// <summary>
/// Represents an email address.
/// </summary>
public sealed record EmailAddress(
    string Name,
    string Address);
