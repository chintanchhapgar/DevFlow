namespace DevFlow.SharedKernel.Results;

/// <summary>
/// Categorizes domain errors to enable appropriate HTTP response mapping.
/// </summary>
public enum ErrorType
{
    None = 0,

    Validation = 1,

    Failure = 2,

    Unauthorized = 3,

    Forbidden = 4,

    NotFound = 5,

    Conflict = 6,

    Unexpected = 7
}

