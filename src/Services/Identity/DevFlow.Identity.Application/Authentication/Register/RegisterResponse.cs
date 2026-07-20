namespace DevFlow.Identity.Application.Authentication.Register;

/// <summary>
/// Response returned after successful registration.
/// </summary>
public sealed record RegisterResponse(
    Guid UserId,
    string Email,
    string FirstName,
    string LastName);
