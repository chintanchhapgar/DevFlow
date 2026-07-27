using DevFlow.SharedKernel.Domain;

namespace DevFlow.Project.Domain.Projects.ValueObjects;

public sealed record ProjectId(Guid Value)
    : StronglyTypedId<Guid>(Value)
{
    public static ProjectId New() => new(Guid.NewGuid());

    public static ProjectId Empty() => new(Guid.Empty);

    public override string ToString() => Value.ToString();
}
