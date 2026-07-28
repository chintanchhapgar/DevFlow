using DevFlow.Project.Domain.WorkItems.ValueObjects;
using DevFlow.SharedKernel.Domain;

namespace DevFlow.Project.Domain.WorkItems.Events;

public sealed record WorkItemCreatedDomainEvent(
    WorkItemId WorkItemId)
    : IDomainEvent;
