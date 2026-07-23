namespace DevFlow.BuildingBlocks.Api.Responses;

public sealed class ApiResponse<T>
{
    public bool Success { get; init; }

    public string Message { get; init; } = string.Empty;

    public T? Data { get; init; }

    public ApiError? Error { get; init; }

    public string TraceId { get; init; } = string.Empty;

    public DateTime Timestamp { get; init; } = DateTime.UtcNow;
}
