using DevFlow.Project.Domain.Epics.ValueObjects;
using DevFlow.SharedKernel.Domain;

namespace DevFlow.Project.Domain.Epics.Events;

public sealed record EpicDeletedDomainEvent(
    EpicId EpicId)
    : IDomainEvent;
