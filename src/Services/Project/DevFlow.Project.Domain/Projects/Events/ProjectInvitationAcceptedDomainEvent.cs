using DevFlow.Project.Domain.Projects.ValueObjects;
using DevFlow.SharedKernel.Domain;

namespace DevFlow.Project.Domain.Projects.Events;

public sealed record ProjectInvitationAcceptedDomainEvent(
    ProjectId ProjectId,
    Guid InvitationId,
    Guid UserId)
    : IDomainEvent;
