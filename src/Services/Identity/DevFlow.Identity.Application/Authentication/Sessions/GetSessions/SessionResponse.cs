using DevFlow.BuildingBlocks.Api.Responses;

namespace DevFlow.Identity.Application.Authentication.Sessions.GetSessions;

public sealed record SessionResponse(
    Guid SessionId,
    string? DeviceName,
    string? Browser,
    string? OperatingSystem,
    string? IpAddress,
    DateTime CreatedOnUtc,
    DateTime? LastUsedOnUtc,
    DateTime ExpiresOnUtc,
    bool IsCurrent)
    : IApiMessage
{
    public string Message => "Active sessions retrieved successfully.";
}
