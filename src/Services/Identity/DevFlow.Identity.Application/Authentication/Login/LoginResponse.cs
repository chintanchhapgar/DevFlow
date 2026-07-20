namespace DevFlow.Identity.Application.Authentication.Login;

/// <summary>
/// Contains the JWT access token and refresh token after successful login.
/// </summary>
public sealed record LoginResponse(
    string AccessToken,
    string RefreshToken,
    DateTime RefreshTokenExpiresAtUtc,
    Guid UserId,
    string Email,
    string FullName);
