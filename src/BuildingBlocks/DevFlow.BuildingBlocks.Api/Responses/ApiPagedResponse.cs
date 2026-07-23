namespace DevFlow.BuildingBlocks.Api.Responses;

public sealed class ApiPagedResponse<T>
{
    public bool Success { get; init; }

    public string Message { get; init; } = string.Empty;

    public IReadOnlyCollection<T> Data { get; init; } = [];

    public int Page { get; init; }

    public int PageSize { get; init; }

    public int TotalCount { get; init; }

    public string TraceId { get; init; } = string.Empty;
}
