namespace DevFlow.Identity.Infrastructure.Options;

public sealed class EmailOptions
{
    public const string SectionName = "Email";

    public string Host { get; init; } = string.Empty;

    public int Port { get; init; }

    public bool UseSsl { get; init; }

    public string SenderName { get; init; } = string.Empty;

    public string SenderEmail { get; init; } = string.Empty;

    public string Username { get; init; } = string.Empty;

    public string Password { get; init; } = string.Empty;
}
