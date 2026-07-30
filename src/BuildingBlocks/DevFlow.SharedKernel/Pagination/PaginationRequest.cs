namespace DevFlow.SharedKernel.Pagination;

public sealed record PaginationRequest
{
    private const int DefaultPageSize = 20;
    private const int MaxPageSize = 100;

    public int Page { get; init; } = 1;

    public int PageSize { get; init; } = DefaultPageSize;

    public string? SortBy { get; init; }

    public string? SortDirection { get; init; } = "asc";

    public int Skip =>
    (GetValidPage() - 1) * GetValidPageSize();

    public int GetValidPage() =>
        Math.Max(1, Page);

    public int GetValidPageSize() =>
        Math.Min(
            Math.Max(1, PageSize),
            MaxPageSize);

    public bool IsDescending =>
        string.Equals(
            SortDirection,
            "desc",
            StringComparison.OrdinalIgnoreCase);
}
