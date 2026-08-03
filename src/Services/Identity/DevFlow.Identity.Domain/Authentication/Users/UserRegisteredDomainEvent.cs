using DevFlow.SharedKernel.Domain.DomainEvents;

namespace DevFlow.Identity.Domain.Authentication.Users;

/// <summary>
/// Raised when a new user is registered.
/// </summary>
public sealed record UserRegisteredDomainEvent(
    UserId UserId,
    string Email,
    string FirstName,
    string LastName)
    : DomainEvent;
