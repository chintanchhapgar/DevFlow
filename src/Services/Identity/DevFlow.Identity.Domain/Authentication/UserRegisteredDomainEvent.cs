using DevFlow.SharedKernel.Domain;

namespace DevFlow.Identity.Domain.Authentication;

/// <summary>
/// Raised when a new user is registered.
/// </summary>
public sealed record UserRegisteredDomainEvent(UserId UserId) : IDomainEvent;
