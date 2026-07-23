using DevFlow.Identity.Domain.Authentication.Users;

namespace DevFlow.Identity.Application.Authentication.Profile;

/// <summary>
/// Current authenticated user.
/// </summary>
public sealed record ProfileResponse(
    UserId UserId,
    string Email,
    string FullName,
    string Role);
