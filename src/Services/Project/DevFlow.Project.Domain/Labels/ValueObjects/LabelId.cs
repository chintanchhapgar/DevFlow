using DevFlow.SharedKernel.Domain;

namespace DevFlow.Project.Domain.Labels.ValueObjects;

public sealed record LabelId(Guid Value)
    : StronglyTypedId<Guid>(Value)
{
    public static LabelId New() => new(Guid.NewGuid());

    public static LabelId Empty() => new(Guid.Empty);
}
