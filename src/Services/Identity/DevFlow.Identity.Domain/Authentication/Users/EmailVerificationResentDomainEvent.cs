using DevFlow.SharedKernel.Domain.DomainEvents;

namespace DevFlow.Identity.Domain.Authentication.Users;

/// <summary>
/// Raised when a user requests another email verification email.
/// </summary>
public sealed record EmailVerificationResentDomainEvent(
    UserId UserId,
    string Email,
    string FirstName,
    Guid VerificationToken)
    : DomainEvent;
