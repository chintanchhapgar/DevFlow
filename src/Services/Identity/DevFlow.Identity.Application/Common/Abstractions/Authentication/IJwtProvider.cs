using DevFlow.Identity.Domain.Authentication;

namespace DevFlow.Identity.Application.Common.Abstractions.Authentication;

/// <summary>
/// Generates JWT access tokens for authenticated users.
/// </summary>
public interface IJwtProvider
{
    string GenerateToken(User user);
}
