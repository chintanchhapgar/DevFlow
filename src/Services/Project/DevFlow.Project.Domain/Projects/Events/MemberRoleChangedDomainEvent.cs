using DevFlow.SharedKernel.Domain.DomainEvents;
using DevFlow.Project.Domain.Projects.Enums;
using DevFlow.Project.Domain.Projects.ValueObjects;

namespace DevFlow.Project.Domain.Projects.Events;

public sealed record MemberRoleChangedDomainEvent(
    ProjectId ProjectId,
    Guid UserId,
    ProjectRole Role)
    : IDomainEvent;
