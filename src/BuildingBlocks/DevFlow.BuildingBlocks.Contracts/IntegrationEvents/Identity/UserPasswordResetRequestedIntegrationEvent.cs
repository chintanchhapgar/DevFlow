using DevFlow.BuildingBlocks.Messaging.IntegrationEvents;

namespace DevFlow.BuildingBlocks.Contracts.IntegrationEvents.Identity;

public sealed record UserPasswordResetRequestedIntegrationEvent(
    Guid UserId,
    string Email,
    string FirstName,
    string ResetToken)
    : IntegrationEvent;
