using DevFlow.SharedKernel.Domain.DomainEvents;
using DevFlow.Project.Domain.Attachments.ValueObjects;

namespace DevFlow.Project.Domain.Attachments.Events;

public sealed record AttachmentDeletedDomainEvent(
    AttachmentId AttachmentId)
    : IDomainEvent;
