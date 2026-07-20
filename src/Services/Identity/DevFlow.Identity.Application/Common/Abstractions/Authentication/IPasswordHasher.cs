namespace DevFlow.Identity.Application.Common.Abstractions.Authentication;

/// <summary>
/// Provides password hashing and verification.
/// </summary>
public interface IPasswordHasher
{
    string Hash(string password);
    bool Verify(string password, string hash);
}
