using DevFlow.Project.Domain.Projects.ValueObjects;
using DevFlow.SharedKernel.Domain;

namespace DevFlow.Project.Domain.Projects.Events;

public sealed record ProjectCreatedDomainEvent(
    ProjectId ProjectId)
    : IDomainEvent;
