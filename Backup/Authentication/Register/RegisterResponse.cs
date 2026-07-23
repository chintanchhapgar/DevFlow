namespace DevFlow.Identity.Application.Authentication.Register;

/// <summary>
/// Registration result.
/// </summary>
public sealed record RegisterResponse(
    Guid UserId,
    string Email);
