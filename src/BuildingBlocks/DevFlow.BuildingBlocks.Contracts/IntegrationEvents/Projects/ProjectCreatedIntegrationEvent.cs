using DevFlow.BuildingBlocks.Messaging.IntegrationEvents;

namespace DevFlow.BuildingBlocks.Contracts.IntegrationEvents.Projects;

/// <summary>
/// Published when a new project is created.
/// </summary>
public sealed record ProjectCreatedIntegrationEvent(
    Guid ProjectId,
    Guid OwnerId,
    string Name,
    string? Description)
    : IntegrationEvent;
