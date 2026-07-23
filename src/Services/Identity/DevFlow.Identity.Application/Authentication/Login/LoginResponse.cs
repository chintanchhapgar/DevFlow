namespace DevFlow.Identity.Application.Authentication.Login;

/// <summary>
/// Login result.
/// </summary>
public sealed record LoginResponse(
    string AccessToken,
    string RefreshToken,
    DateTime RefreshTokenExpiresOnUtc);
