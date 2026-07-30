namespace DevFlow.SharedKernel.Pagination;

public sealed class PagedList<T>
{
    public PagedList(
        IReadOnlyList<T> items,
        int page,
        int pageSize,
        int totalCount)
    {
        Items = items;
        Page = page;
        PageSize = pageSize;
        TotalCount = totalCount;
    }

    public IReadOnlyList<T> Items { get; }

    public int Page { get; }

    public int PageSize { get; }

    public int TotalCount { get; }

    public int TotalPages =>
        (int)Math.Ceiling(TotalCount / (double)PageSize);

    public bool HasNextPage =>
        Page < TotalPages;

    public bool HasPreviousPage =>
        Page > 1;

    public PagedList<TResult> Map<TResult>(
        Func<T, TResult> mapper)
    {
        return new PagedList<TResult>(
            Items.Select(mapper).ToList(),
            Page,
            PageSize,
            TotalCount);
    }
}
