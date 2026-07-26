namespace DevFlow.Identity.Application.Authentication.Sessions.RevokeSession;

public sealed record RevokeSessionResponse(
    Guid SessionId,
    string Message);
