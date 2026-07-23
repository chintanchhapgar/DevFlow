namespace DevFlow.Identity.Application.Common.Abstractions.Authentication;

/// <summary>
/// Provides password hashing services.
/// </summary>
public interface IPasswordHasher
{
    string Hash(string password);

    bool Verify(
        string password,
        string passwordHash);
}
