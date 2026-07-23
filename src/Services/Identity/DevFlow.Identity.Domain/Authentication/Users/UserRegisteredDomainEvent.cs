using DevFlow.SharedKernel.Domain;

namespace DevFlow.Authentication.Users;

/// <summary>
/// Raised when a new user is registered.
/// </summary>
public sealed record UserRegisteredDomainEvent(UserId UserId) : IDomainEvent;
