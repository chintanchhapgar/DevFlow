namespace DevFlow.Identity.Application.Authentication.Login;

/// <summary>
/// Login result.
/// </summary>
public sealed record LoginResponse(
    Guid UserId,
    string AccessToken);
