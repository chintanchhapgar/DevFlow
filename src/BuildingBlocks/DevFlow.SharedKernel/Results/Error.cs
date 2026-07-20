namespace DevFlow.SharedKernel.Results;

/// <summary>
/// Represents a domain error with a code and description.
/// Used within the Result pattern to avoid exceptions for expected failures.
/// </summary>
public sealed record Errors(string Code, string Description, ErrorType Type = ErrorType.Failure)
{
    /// <summary>
    /// Represents no error (success state).
    /// </summary>
    public static readonly Errors None = new(string.Empty, string.Empty, ErrorType.None);

    /// <summary>
    /// A null value was provided where a value was required.
    /// </summary>
    public static readonly Errors NullValue = new(
        "General.NullValue",
        "A null value was provided.",
        ErrorType.Failure);

    public static Errors Failure(string code, string description) =>
        new(code, description, ErrorType.Failure);

    public static Errors NotFound(string code, string description) =>
        new(code, description, ErrorType.NotFound);

    public static Errors Validation(string code, string description) =>
        new(code, description, ErrorType.Validation);

    public static Errors Conflict(string code, string description) =>
        new(code, description, ErrorType.Conflict);

    public static Errors Unauthorized(string code, string description) =>
        new(code, description, ErrorType.Unauthorized);

    public static Errors Forbidden(string code, string description) =>
        new(code, description, ErrorType.Forbidden);
}
