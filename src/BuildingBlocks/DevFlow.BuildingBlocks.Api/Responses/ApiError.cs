using DevFlow.SharedKernel.Results;

namespace DevFlow.BuildingBlocks.Api.Responses;

/// <summary>
/// Represents an API error.
/// </summary>
public sealed class ApiError
{
    public string Code { get; init; } = string.Empty;

    public ErrorType Type { get; init; }

    public IReadOnlyDictionary<string, string[]>? ValidationErrors { get; init; }

    // Only populated in Development
    public string? Details { get; init; }
}
