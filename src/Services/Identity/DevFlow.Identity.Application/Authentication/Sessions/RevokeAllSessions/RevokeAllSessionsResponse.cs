namespace DevFlow.Identity.Application.Authentication.Sessions.RevokeAllSessions;

public sealed record RevokeAllSessionsResponse(
    int RevokedSessions,
    string Message);
