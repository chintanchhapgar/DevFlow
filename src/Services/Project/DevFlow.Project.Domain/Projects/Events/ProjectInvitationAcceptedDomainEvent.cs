using DevFlow.SharedKernel.Domain.DomainEvents;
using DevFlow.Project.Domain.Projects.ValueObjects;

namespace DevFlow.Project.Domain.Projects.Events;

public sealed record ProjectInvitationAcceptedDomainEvent(
    ProjectId ProjectId,
    Guid InvitationId,
    Guid UserId)
    : IDomainEvent;
