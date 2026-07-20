namespace DevFlow.SharedKernel.Domain;

/// <summary>
/// Base record for strongly-typed IDs.
/// Using records gives us structural equality for free.
/// </summary>
/// <typeparam name="TValue">The underlying primitive type (typically Guid).</typeparam>
public abstract record StronglyTypedId<TValue>(TValue Value)
    where TValue : notnull
{
    public override string ToString() => Value.ToString()!;
}
