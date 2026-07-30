using DevFlow.SharedKernel.Domain;

namespace DevFlow.Project.Domain.Attachments.ValueObjects;

public sealed record AttachmentId(Guid Value)
    : StronglyTypedId<Guid>(Value)
{
    public static AttachmentId New() => new(Guid.NewGuid());

    public static AttachmentId Empty() => new(Guid.Empty);
}
