namespace DevFlow.SharedKernel.Pagination;

/// <summary>
/// Standard pagination request parameters with sensible defaults and limits.
/// </summary>
public sealed record PaginationParams
{
    private const int DefaultPageSize = 20;
    private const int MaxPageSize = 100;

    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = DefaultPageSize;

    public int GetValidPage() => Math.Max(1, Page);

    public int GetValidPageSize() => Math.Min(
        Math.Max(1, PageSize),
        MaxPageSize);
}
