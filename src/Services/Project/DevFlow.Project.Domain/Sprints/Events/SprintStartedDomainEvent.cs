using DevFlow.SharedKernel.Domain.DomainEvents;
using DevFlow.Project.Domain.Sprints.ValueObjects;

namespace DevFlow.Project.Domain.Sprints.Events;

public sealed record SprintStartedDomainEvent(
    SprintId SprintId)
    : IDomainEvent;
