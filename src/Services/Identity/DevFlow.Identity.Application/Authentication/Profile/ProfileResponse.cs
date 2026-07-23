namespace DevFlow.Identity.Application.Authentication.Profile;

/// <summary>
/// Current authenticated user.
/// </summary>
public sealed record ProfileResponse(
    Guid UserId,
    string Email,
    string FullName,
    string Role);
