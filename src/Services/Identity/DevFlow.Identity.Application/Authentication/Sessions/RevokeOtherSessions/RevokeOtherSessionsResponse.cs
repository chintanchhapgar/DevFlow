namespace DevFlow.Identity.Application.Authentication.Sessions.RevokeOtherSessions;

public sealed record RevokeOtherSessionsResponse(
    int RevokedSessions,
    string Message);
