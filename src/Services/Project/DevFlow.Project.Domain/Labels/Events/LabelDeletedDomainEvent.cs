using DevFlow.SharedKernel.Domain.DomainEvents;
using DevFlow.Project.Domain.Labels.ValueObjects;

namespace DevFlow.Project.Domain.Labels.Events;

public sealed record LabelDeletedDomainEvent(
    LabelId LabelId)
    : IDomainEvent;
