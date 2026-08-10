using DevFlow.BuildingBlocks.Messaging.IntegrationEvents;

namespace DevFlow.BuildingBlocks.Contracts.IntegrationEvents.Users;

public sealed record UserEmailVerificationResentIntegrationEvent(
    Guid UserId,
    string Email,
    string FirstName,
    Guid VerificationToken)
    : IntegrationEvent;
