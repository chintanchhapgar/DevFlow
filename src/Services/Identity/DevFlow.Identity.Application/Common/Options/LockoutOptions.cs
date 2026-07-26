namespace DevFlow.Identity.Application.Common.Abstractions.Options;

public sealed class LockoutOptions
{
    public const string SectionName = "Lockout";

    public int MaxFailedAttempts { get; init; } = 5;

    public int DurationMinutes { get; init; } = 15;
}
