using DevFlow.Identity.Domain.Authentication.Users;

namespace DevFlow.Identity.Domain.Authentication.SecurityEvents;

public sealed class SecurityEvent
    : Entity<SecurityEventId>
{
    private SecurityEvent(
        SecurityEventId id,
        UserId userId,
        SecurityEventType eventType,
        string? ipAddress,
        string? userAgent,
        string? deviceName,
        string? browser,
        string? operatingSystem,
        string? details)
        : base(id)
    {
        UserId = userId;
        EventType = eventType;

        IpAddress = ipAddress;
        UserAgent = userAgent;
        DeviceName = deviceName;
        Browser = browser;
        OperatingSystem = operatingSystem;

        Details = details;

        OccurredOnUtc = DateTime.UtcNow;
    }

    private SecurityEvent()
    {
    }

    public UserId UserId { get; private set; } = default!;

    public SecurityEventType EventType { get; private set; }

    public string? IpAddress { get; private set; }

    public string? UserAgent { get; private set; }

    public string? DeviceName { get; private set; }

    public string? Browser { get; private set; }

    public string? OperatingSystem { get; private set; }

    public string? Details { get; private set; }

    public DateTime OccurredOnUtc { get; private set; }

    public static SecurityEvent Create(
        UserId userId,
        SecurityEventType eventType,
        string? ipAddress = null,
        string? userAgent = null,
        string? deviceName = null,
        string? browser = null,
        string? operatingSystem = null,
        string? details = null)
    {
        return new SecurityEvent(
            SecurityEventId.New(),
            userId,
            eventType,
            ipAddress,
            userAgent,
            deviceName,
            browser,
            operatingSystem,
            details);
    }
}
