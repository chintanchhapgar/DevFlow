using DevFlow.BuildingBlocks.Messaging.IntegrationEvents;

namespace DevFlow.BuildingBlocks.Contracts.IntegrationEvents.Identity;

public sealed record UserRegisteredIntegrationEvent(
    Guid UserId,
    string Email,
    string FirstName,
    string LastName)
    : IntegrationEvent;
