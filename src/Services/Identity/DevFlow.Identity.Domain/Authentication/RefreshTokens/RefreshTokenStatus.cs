namespace DevFlow.Identity.Domain.Authentication.RefreshTokens;

/// <summary>
/// Refresh token status.
/// </summary>
public enum RefreshTokenStatus
{
    Active = 1,
    Revoked = 2,
    Expired = 3
}
