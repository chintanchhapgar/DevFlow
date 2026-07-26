namespace DevFlow.Identity.Application.Authentication.SecurityEvents;

public sealed record SecurityEventResponse(
    Guid Id,
    string Event,
    string? Device,
    string? Browser,
    string? OperatingSystem,
    string? IpAddress,
    string? Details,
    DateTime OccurredOnUtc);
