using DevFlow.Project.Domain.Projects.ValueObjects;
using DevFlow.SharedKernel.Abstractions;
using DevFlow.SharedKernel.Domain;

namespace DevFlow.Project.Domain.Projects.Events;

public sealed record ProjectInvitationRevokedDomainEvent(
    ProjectId ProjectId,
    Guid InvitationId)
    : IDomainEvent;
