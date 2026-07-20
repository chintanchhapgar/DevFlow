using DevFlow.SharedKernel.Domain;

namespace DevFlow.Identity.Domain.Authentication.Events;

/// <summary>
/// Raised when a user updates their profile information.
/// </summary>
public sealed record UserProfileUpdatedDomainEvent(
    UserId UserId,
    string FirstName,
    string LastName) : DomainEvent;
