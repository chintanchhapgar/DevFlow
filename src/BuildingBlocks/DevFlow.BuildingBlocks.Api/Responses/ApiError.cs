namespace DevFlow.BuildingBlocks.Api.Responses;

public sealed class ApiError
{
    public string Code { get; init; } = string.Empty;

    public string Message { get; init; } = string.Empty;

    public string Type { get; init; } = string.Empty;

    public IReadOnlyList<ApiValidationError>? ValidationErrors { get; init; }
}
