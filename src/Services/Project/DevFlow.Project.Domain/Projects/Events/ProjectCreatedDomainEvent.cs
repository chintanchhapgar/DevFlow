using DevFlow.Project.Domain.Projects.ValueObjects;
using DevFlow.SharedKernel.Domain.DomainEvents;

namespace DevFlow.Project.Domain.Projects;

/// <summary>
/// Raised when a project is created.
/// </summary>
public sealed record ProjectCreatedDomainEvent(
    ProjectId ProjectId,
    Guid OwnerId,
    string Name,
    string? Description)
    : DomainEvent;
