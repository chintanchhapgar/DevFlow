using DevFlow.Project.Domain.Projects.ValueObjects;
using DevFlow.SharedKernel.Domain;

namespace DevFlow.Project.Domain.Projects.Events;

public sealed record ProjectUpdatedDomainEvent(
    ProjectId ProjectId)
    : IDomainEvent;
