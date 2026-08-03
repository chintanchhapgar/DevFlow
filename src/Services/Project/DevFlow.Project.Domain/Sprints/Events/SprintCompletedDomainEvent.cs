using DevFlow.SharedKernel.Domain.DomainEvents;
using DevFlow.Project.Domain.Sprints.ValueObjects;

namespace DevFlow.Project.Domain.Sprints.Events;

public sealed record SprintCompletedDomainEvent(
    SprintId SprintId)
    : IDomainEvent;
