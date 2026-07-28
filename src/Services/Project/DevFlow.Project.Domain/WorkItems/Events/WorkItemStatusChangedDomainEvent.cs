using DevFlow.Project.Domain.WorkItems.Enums;
using DevFlow.Project.Domain.WorkItems.ValueObjects;
using DevFlow.SharedKernel.Domain;

namespace DevFlow.Project.Domain.WorkItems.Events;

public sealed record WorkItemStatusChangedDomainEvent(
    WorkItemId WorkItemId,
    WorkItemStatus Status)
    : IDomainEvent;
