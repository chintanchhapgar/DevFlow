using DevFlow.SharedKernel.Domain.DomainEvents;
using DevFlow.Project.Domain.Worklogs.ValueObjects;

namespace DevFlow.Project.Domain.Worklogs.Events;

public sealed record WorklogUpdatedDomainEvent(
    WorklogId WorklogId)
    : IDomainEvent;
