using DevFlow.Project.Domain.Worklogs.ValueObjects;
using DevFlow.SharedKernel.Domain;
namespace DevFlow.Project.Domain.Worklogs.Events;

public sealed record WorklogDeletedDomainEvent(
    WorklogId WorklogId)
    : IDomainEvent;
