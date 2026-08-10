namespace DevFlow.Notification.Infrastructure.Email.Configuration;

public sealed class EmailSettings
{
    public const string SectionName = "Email";

    public string VerificationBaseUrl { get; init; } = string.Empty;
}
