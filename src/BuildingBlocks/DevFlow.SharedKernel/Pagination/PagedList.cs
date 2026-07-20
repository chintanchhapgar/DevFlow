namespace DevFlow.SharedKernel.Pagination;

/// <summary>
/// Represents a paginated list of items.
/// </summary>
/// <typeparam name="T">The item type.</typeparam>
public sealed class PagedList<T>
{
    private PagedList(
        List<T> items,
        int page,
        int pageSize,
        int totalCount)
    {
        Items = items;
        Page = page;
        PageSize = pageSize;
        TotalCount = totalCount;
    }

    public List<T> Items { get; }
    public int Page { get; }
    public int PageSize { get; }
    public int TotalCount { get; }
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
    public bool HasNextPage => Page < TotalPages;
    public bool HasPreviousPage => Page > 1;

    public  PagedList<T> Create(List<T> items, int page, int pageSize, int totalCount)
    {
        return new PagedList<T>(items, page, pageSize, totalCount);
    }

    /// <summary>
    /// Projects each item using a mapping function.
    /// </summary>
    public PagedList<TResult> Map<TResult>(Func<T, TResult> mapper)
    {
        var mappedItems = Items.Select(mapper).ToList();
        return new PagedList<TResult>(mappedItems, Page, PageSize, TotalCount);
    }
}
