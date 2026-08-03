

using DevFlow.BuildingBlocks.Messaging.IntegrationEvents;

namespace DevFlow.Identity.Contracts.IntegrationEvents;

/// <summary>
/// Published when a user has been successfully registered.
/// </summary>
public sealed record UserRegisteredIntegrationEvent(
    Guid UserId,
    string Email,
    string FirstName,
    string LastName)
    : IntegrationEvent;
