namespace DevFlow.SharedKernel.Pagination;

/// <summary>
/// Standard pagination request.
/// </summary>
public sealed record PaginationRequest
{
    private const int DefaultPage = 1;
    private const int DefaultPageSize = 20;
    private const int MaxPageSize = 100;

    public int Page { get; init; } = DefaultPage;

    public int PageSize { get; init; } = DefaultPageSize;

    public int GetValidPage() =>
        Math.Max(DefaultPage, Page);

    public int GetValidPageSize() =>
        Math.Clamp(PageSize, 1, MaxPageSize);

    public int Skip =>
        (GetValidPage() - 1) * GetValidPageSize();
}
