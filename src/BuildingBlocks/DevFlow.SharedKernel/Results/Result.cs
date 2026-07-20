namespace DevFlow.SharedKernel.Results;

/// <summary>
/// Represents the outcome of an operation that may succeed or fail.
/// Avoids using exceptions for expected domain failures.
/// </summary>
public class Result
{
    protected Result(bool isSuccess, Errors error)
    {
        if (isSuccess && error != Errors.None)
            throw new InvalidOperationException("A successful result cannot have an error.");

        if (!isSuccess && error == Errors.None)
            throw new InvalidOperationException("A failed result must have an error.");

        IsSuccess = isSuccess;
        Error = error;
    }

    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Errors Error { get; }

    public static Result Success() => new(true, Errors.None);

    public static Result<TValue> Success<TValue>(TValue value) =>
        new(value, true, Errors.None);

    public static Result Failure(Errors error) => new(false, error);

    public static Result<TValue> Failure<TValue>(Errors error) =>
        new(default, false, error);

    public static Result<TValue> Create<TValue>(TValue? value) =>
        value is not null
            ? Success(value)
            : Failure<TValue>(Errors.NullValue);

    /// <summary>
    /// Implicitly converts an Error to a failed Result.
    /// Enables: return SomeError;
    /// </summary>
    public static implicit operator Result(Errors error) => Failure(error);
}
