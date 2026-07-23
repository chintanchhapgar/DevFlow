namespace DevFlow.SharedKernel.Results;

/// <summary>
/// Represents the outcome of an operation that returns a value on success.
/// </summary>
/// <typeparam name="TValue">The type of the success value.</typeparam>
public sealed class Result<TValue> : Result
{
    private readonly TValue? _value;

    internal Result(TValue? value, bool isSuccess, AppError error)
        : base(isSuccess, error)
    {
        _value = value;
    }

    /// <summary>
    /// Gets the value if the result is successful.
    /// Throws InvalidOperationException if the result is a failure.
    /// </summary>
    public TValue Value =>
        IsSuccess
            ? _value!
            : throw new InvalidOperationException(
                $"Cannot access the value of a failed result. Error: {Error.Code}");

    /// <summary>
    /// Implicitly converts a value to a successful Result.
    /// Enables: return someValue;
    /// </summary>
    public static implicit operator Result<TValue>(TValue value) =>
        Success(value);

    /// <summary>
    /// Implicitly converts an Error to a failed Result.
    /// Enables: return SomeError;
    /// </summary>
    public static implicit operator Result<TValue>(AppError error) =>
        Failure<TValue>(error);
}
