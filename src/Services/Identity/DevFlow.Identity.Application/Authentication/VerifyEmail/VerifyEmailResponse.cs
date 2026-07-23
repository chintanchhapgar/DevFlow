namespace DevFlow.Identity.Application.Authentication.VerifyEmail;

/// <summary>
/// Response returned after successful email verification.
/// </summary>
public sealed record VerifyEmailResponse(
    Guid UserId,
    string Message);
