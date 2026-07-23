namespace DevFlow.SharedKernel.Results;

/// <summary>
/// Represents a domain error with a code and description.
/// Used within the Result pattern to avoid exceptions for expected failures.
/// </summary>
public sealed record AppError(string Code, string Description, ErrorType Type = ErrorType.Failure)
{
    /// <summary>
    /// Represents no error (success state).
    /// </summary>
    public static readonly AppError None = new(string.Empty, string.Empty, ErrorType.None);

    /// <summary>
    /// A null value was provided where a value was required.
    /// </summary>
    public static readonly AppError NullValue = new(
        "General.NullValue",
        "A null value was provided.",
        ErrorType.Failure);

    public static AppError Failure(string code, string description) =>
        new(code, description, ErrorType.Failure);

    public static AppError NotFound(string code, string description) =>
        new(code, description, ErrorType.NotFound);

    public static AppError Validation(string code, string description) =>
        new(code, description, ErrorType.Validation);

    public static AppError Conflict(string code, string description) =>
        new(code, description, ErrorType.Conflict);

    public static AppError Unauthorized(string code, string description) =>
        new(code, description, ErrorType.Unauthorized);

    public static AppError Forbidden(string code, string description) =>
        new(code, description, ErrorType.Forbidden);
}
