using DevFlow.SharedKernel.Domain;

namespace DevFlow.Project.Domain.Epics.ValueObjects;

public sealed record EpicId(Guid Value)
    : StronglyTypedId<Guid>(Value)
{
    public static EpicId New() => new(Guid.NewGuid());

    public static EpicId Empty() => new(Guid.Empty);
}
