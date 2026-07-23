using DevFlow.SharedKernel.Domain;

namespace DevFlow.Identity.Domain.Authentication;

public sealed record UserId(Guid Value)
    : StronglyTypedId<Guid>(Value)
{
    public static UserId New() => new(Guid.NewGuid());

    public static UserId Empty => new(Guid.Empty);
}
