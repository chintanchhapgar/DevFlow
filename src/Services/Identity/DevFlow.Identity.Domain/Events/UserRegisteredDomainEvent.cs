using DevFlow.SharedKernel.Domain;

namespace DevFlow.Identity.Domain.Authentication.Events;

/// <summary>
/// Raised when a new user successfully registers.
/// Triggers welcome email, user projection creation, etc.
/// </summary>
public sealed record UserRegisteredDomainEvent(
    UserId UserId,
    string Email,
    string FirstName,
    string LastName) : DomainEvent;
