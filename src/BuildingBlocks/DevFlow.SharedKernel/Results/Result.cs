namespace DevFlow.SharedKernel.Results;

/// <summary>
/// Represents the outcome of an operation that may succeed or fail.
/// Avoids using exceptions for expected domain failures.
/// </summary>
public class Result
{
    protected Result(
        bool isSuccess,
        AppError error,
        string message = "")
    {
        if (isSuccess && error != AppError.None)
            throw new InvalidOperationException(
                "A successful result cannot have an error.");

        if (!isSuccess && error == AppError.None)
            throw new InvalidOperationException(
                "A failed result must have an error.");

        IsSuccess = isSuccess;
        Error = error;
        Message = message;
    }

    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public AppError Error { get; }

    public string Message { get; }

    public static Result Success(
        string message = "") =>
        new(true, AppError.None, message);

    public static Result<TValue> Success<TValue>(
        TValue value,
        string message = "") =>
        new(value, true, AppError.None, message);

    public static Result Failure(
        AppError error) =>
        new(false, error);

    public static Result<TValue> Failure<TValue>(
        AppError error) =>
        new(default, false, error);

    public static Result<TValue> Create<TValue>(
        TValue? value) =>
        value is not null
            ? Success(value)
            : Failure<TValue>(AppError.NullValue);

    public static implicit operator Result(
        AppError error) =>
        Failure(error);
}
