using DevFlow.SharedKernel.Domain.DomainEvents;
using DevFlow.Project.Domain.WorkItems.Enums;
using DevFlow.Project.Domain.WorkItems.ValueObjects;

namespace DevFlow.Project.Domain.WorkItems.Events;

public sealed record WorkItemPriorityChangedDomainEvent(
    WorkItemId WorkItemId,
    WorkItemPriority Priority)
    : IDomainEvent;
