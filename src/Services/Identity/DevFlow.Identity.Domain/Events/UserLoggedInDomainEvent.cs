using DevFlow.SharedKernel.Domain;

namespace DevFlow.Identity.Domain.Authentication.Events;

/// <summary>
/// Raised after a successful login.
/// Can be used for audit logging, session tracking, etc.
/// </summary>
public sealed record UserLoggedInDomainEvent(
    UserId UserId,
    string Email,
    DateTime LoggedInAtUtc) : DomainEvent;
