using DevFlow.SharedKernel.Domain;

namespace DevFlow.Project.Domain.WorkItems.ValueObjects;

public sealed record WorkItemId(Guid Value)
    : StronglyTypedId<Guid>(Value)
{
    public static WorkItemId New() => new(Guid.NewGuid());

    public static WorkItemId Empty() => new(Guid.Empty);
}
