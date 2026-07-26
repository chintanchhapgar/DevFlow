namespace DevFlow.Identity.Domain.Authentication.SecurityEvents;

public sealed record SecurityEventId(Guid Value)
{
    public static SecurityEventId New() => new(Guid.NewGuid());

    public static readonly SecurityEventId Empty =
        new(Guid.Empty);

    public override string ToString() => Value.ToString();
}
