using DevFlow.SharedKernel.Domain;

namespace DevFlow.Project.Domain.Comments.ValueObjects;

public sealed record CommentId(Guid Value)
    : StronglyTypedId<Guid>(Value)
{
    public static CommentId New() => new(Guid.NewGuid());

    public static CommentId Empty() => new(Guid.Empty);
}
