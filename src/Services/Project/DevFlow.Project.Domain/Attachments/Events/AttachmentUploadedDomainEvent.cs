using DevFlow.Project.Domain.Attachments.ValueObjects;
using DevFlow.SharedKernel.Domain;

namespace DevFlow.Project.Domain.Attachments.Events;

public sealed record AttachmentUploadedDomainEvent(
    AttachmentId AttachmentId)
    : IDomainEvent;
