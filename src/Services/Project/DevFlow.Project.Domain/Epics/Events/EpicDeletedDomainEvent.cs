using DevFlow.SharedKernel.Domain.DomainEvents;
using DevFlow.Project.Domain.Epics.ValueObjects;

namespace DevFlow.Project.Domain.Epics.Events;

public sealed record EpicDeletedDomainEvent(
    EpicId EpicId)
    : IDomainEvent;
