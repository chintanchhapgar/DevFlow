using DevFlow.SharedKernel.Domain;

namespace DevFlow.Project.Domain.Sprints.ValueObjects;

public sealed record SprintId(Guid Value)
    : StronglyTypedId<Guid>(Value)
{
    public static SprintId New() =>
        new(Guid.NewGuid());

    public static SprintId Empty() =>
        new(Guid.Empty);
}
