using DevFlow.SharedKernel.Abstractions;

namespace DevFlow.Identity.Domain.Authentication.RefreshTokens;

/// <summary>
/// Strongly typed identifier for refresh tokens.
/// </summary>
public sealed record RefreshTokenId(Guid Value)
    : StronglyTypedId<Guid>(Value)
{
    public static RefreshTokenId New() => new(Guid.NewGuid());
}
