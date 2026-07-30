namespace DevFlow.SharedKernel.Pagination;

public sealed record SortingRequest
{
    public string? SortBy { get; init; }

    public string? SortDirection { get; init; } = "asc";

    public bool IsDescending =>
        string.Equals(
            SortDirection,
            "desc",
            StringComparison.OrdinalIgnoreCase);
}
