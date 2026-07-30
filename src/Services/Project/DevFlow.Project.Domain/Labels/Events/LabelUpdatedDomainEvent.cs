using DevFlow.Project.Domain.Labels.ValueObjects;
using DevFlow.SharedKernel.Domain;

namespace DevFlow.Project.Domain.Labels.Events;

public sealed record LabelUpdatedDomainEvent(
    LabelId LabelId)
    : IDomainEvent;
