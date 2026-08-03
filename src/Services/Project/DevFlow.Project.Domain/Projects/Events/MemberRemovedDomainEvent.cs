using DevFlow.SharedKernel.Domain.DomainEvents;
using DevFlow.Project.Domain.Projects.ValueObjects;

namespace DevFlow.Project.Domain.Projects.Events;

public sealed record MemberRemovedDomainEvent(
    ProjectId ProjectId,
    Guid UserId)
    : IDomainEvent;
