using System.Runtime.CompilerServices;

namespace DevFlow.SharedKernel.Guards;

/// <summary>
/// Provides guard clause methods for validating preconditions.
/// Guards throw exceptions for programming errors (not domain errors).
/// Use the Result pattern for expected domain failures.
/// </summary>
public static class Guard
{
    /// <summary>
    /// Ensures a value is not null.
    /// </summary>
    public static T AgainstNull<T>(
        T? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where T : class
    {
        if (value is null)
            throw new ArgumentNullException(paramName, $"{paramName} cannot be null.");

        return value;
    }

    /// <summary>
    /// Ensures a string is not null or whitespace.
    /// </summary>
    public static string AgainstNullOrWhiteSpace(
        string? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException($"{paramName} cannot be null or whitespace.", paramName);

        return value;
    }

    /// <summary>
    /// Ensures a value is within a specified range.
    /// </summary>
    public static T AgainstOutOfRange<T>(
        T value,
        T min,
        T max,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where T : IComparable<T>
    {
        if (value.CompareTo(min) < 0 || value.CompareTo(max) > 0)
            throw new ArgumentOutOfRangeException(
                paramName,
                $"{paramName} must be between {min} and {max}. Actual: {value}");

        return value;
    }

    /// <summary>
    /// Ensures a value is a valid enum member.
    /// </summary>
    public static T AgainstInvalidEnum<T>(
        T value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where T : struct, Enum
    {
        if (!Enum.IsDefined(value))
            throw new ArgumentOutOfRangeException(
                paramName,
                $"{value} is not a valid {typeof(T).Name} value.");

        return value;
    }

    /// <summary>
    /// Ensures a Guid is not empty.
    /// </summary>
    public static Guid AgainstEmptyGuid(
        Guid value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value == Guid.Empty)
            throw new ArgumentException($"{paramName} cannot be an empty Guid.", paramName);

        return value;
    }

    /// <summary>
    /// Ensures a string does not exceed a maximum length.
    /// </summary>
    public static string AgainstMaxLength(
        string value,
        int maxLength,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value.Length > maxLength)
            throw new ArgumentException(
                $"{paramName} cannot exceed {maxLength} characters. Actual: {value.Length}",
                paramName);

        return value;
    }
}
