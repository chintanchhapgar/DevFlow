using DevFlow.SharedKernel.Domain;

namespace DevFlow.Identity.Domain.Authentication.PasswordResetTokens;

public sealed record PasswordResetTokenId(Guid Value)
    : StronglyTypedId<Guid>(Value)
{
    public static PasswordResetTokenId New() =>
        new(Guid.NewGuid());
}
