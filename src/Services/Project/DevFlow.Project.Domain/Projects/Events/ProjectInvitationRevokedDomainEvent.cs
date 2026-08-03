using DevFlow.SharedKernel.Domain.DomainEvents;
using DevFlow.Project.Domain.Projects.ValueObjects;
using DevFlow.SharedKernel.Abstractions;

namespace DevFlow.Project.Domain.Projects.Events;

public sealed record ProjectInvitationRevokedDomainEvent(
    ProjectId ProjectId,
    Guid InvitationId)
    : IDomainEvent;
