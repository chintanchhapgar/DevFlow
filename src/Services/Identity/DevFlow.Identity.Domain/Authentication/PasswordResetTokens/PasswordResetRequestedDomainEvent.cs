
using DevFlow.Identity.Domain.Authentication.Users;


namespace DevFlow.Identity.Domain.Authentication.PasswordResetTokens;

public sealed record PasswordResetRequestedDomainEvent(
    UserId UserId,
    PasswordResetTokenId TokenId)
    : IDomainEvent;
