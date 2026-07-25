using DevFlow.BuildingBlocks.Api.Responses;

namespace DevFlow.Identity.Application.Authentication.Common;

/// <summary>
/// Authentication response.
/// Used by both Login and MFA Login.
/// </summary>
public sealed record AuthenticationResponse(
    bool RequiresTwoFactor,
    Guid? UserId,
    string? AccessToken,
    string? RefreshToken,
    DateTime? RefreshTokenExpiresOnUtc)
    : IApiMessage
{
    public string Message =>
        RequiresTwoFactor
            ? "Two-factor authentication required."
            : "Authentication successful.";

    public static AuthenticationResponse Challenge(Guid userId)
        => new(
            RequiresTwoFactor: true,
            UserId: userId,
            AccessToken: null,
            RefreshToken: null,
            RefreshTokenExpiresOnUtc: null);

    public static AuthenticationResponse Success(
        string accessToken,
        string refreshToken,
        DateTime refreshTokenExpiresOnUtc)
        => new(
            RequiresTwoFactor: false,
            UserId: null,
            AccessToken: accessToken,
            RefreshToken: refreshToken,
            RefreshTokenExpiresOnUtc: refreshTokenExpiresOnUtc);
}
