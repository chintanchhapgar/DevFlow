namespace DevFlow.Identity.Application.Common.Abstractions.Authentication;

/// <summary>
/// Generates JWT access tokens.
/// </summary>
public interface IJwtProvider
{
    string GenerateAccessToken(Guid userId);
}
