namespace DevFlow.SharedKernel.Results;

/// <summary>
/// Categorizes domain errors to enable appropriate HTTP response mapping.
/// </summary>
public enum ErrorType
{
    None = 0,
    Failure = 1,
    Validation = 2,
    NotFound = 3,
    Conflict = 4,
    Unauthorized = 5,
    Forbidden = 6
}
