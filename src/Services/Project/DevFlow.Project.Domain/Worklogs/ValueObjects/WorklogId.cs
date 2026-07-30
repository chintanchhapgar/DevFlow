using DevFlow.SharedKernel.Domain;

namespace DevFlow.Project.Domain.Worklogs.ValueObjects;

public sealed record WorklogId(Guid Value)
    : StronglyTypedId<Guid>(Value)
{
    public static WorklogId New()
        => new(Guid.NewGuid());

    public static WorklogId Empty()
        => new(Guid.Empty);
}
