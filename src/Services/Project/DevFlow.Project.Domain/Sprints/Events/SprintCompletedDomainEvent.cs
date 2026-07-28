using DevFlow.Project.Domain.Sprints.ValueObjects;
using DevFlow.SharedKernel.Domain;

namespace DevFlow.Project.Domain.Sprints.Events;

public sealed record SprintCompletedDomainEvent(
    SprintId SprintId)
    : IDomainEvent;
