namespace DevFlow.Notification.Infrastructure.Email.Configuration;

public sealed class EmailSettings
{
    public const string SectionName = "Email";

    public string FrontendBaseUrl { get; init; } = string.Empty;
}
