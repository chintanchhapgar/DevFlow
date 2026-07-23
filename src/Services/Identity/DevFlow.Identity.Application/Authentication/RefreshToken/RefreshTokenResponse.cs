namespace DevFlow.Identity.Application.Authentication.RefreshToken;

public sealed record RefreshTokenResponse(
    string AccessToken,
    string RefreshToken,
    DateTime ExpiresOnUtc);
