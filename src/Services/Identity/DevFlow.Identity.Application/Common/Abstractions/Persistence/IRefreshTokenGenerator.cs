namespace DevFlow.Identity.Application.Common.Abstractions.Services;

/// <summary>
/// Generates cryptographically secure refresh tokens.
/// </summary>
public interface IRefreshTokenGenerator
{
    string Generate();
    TimeSpan Expiration { get; }
}
