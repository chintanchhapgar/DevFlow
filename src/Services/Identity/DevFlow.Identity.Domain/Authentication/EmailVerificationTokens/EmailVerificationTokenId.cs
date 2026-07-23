using DevFlow.SharedKernel.Domain;

namespace DevFlow.Identity.Domain.Authentication.EmailVerificationTokens;

/// <summary>
/// Strongly typed identifier for EmailVerificationToken.
/// </summary>
public sealed record EmailVerificationTokenId(Guid Value)
    : StronglyTypedId<Guid>(Value)
{
    public static EmailVerificationTokenId New()
        => new(Guid.NewGuid());

    public static EmailVerificationTokenId Empty
        => new(Guid.Empty);
}
