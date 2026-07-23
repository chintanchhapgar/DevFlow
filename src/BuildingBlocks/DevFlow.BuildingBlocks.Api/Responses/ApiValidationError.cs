namespace DevFlow.BuildingBlocks.Api.Responses;

public sealed class ApiValidationError
{
    public string Field { get; init; } = string.Empty;

    public string Message { get; init; } = string.Empty;
}
