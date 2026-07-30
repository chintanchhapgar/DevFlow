using DevFlow.SharedKernel.Pagination;
using Microsoft.EntityFrameworkCore;

namespace DevFlow.BuildingBlocks.Infrastructure.Persistence.Pagination;

public static class IQueryableExtensions
{
    public static async Task<PagedList<T>> ToPagedListAsync<T>(
        this IQueryable<T> query,
        PaginationRequest request,
        CancellationToken cancellationToken = default)
    {
        var page = request.GetValidPage();
        var pageSize = request.GetValidPageSize();

        var totalCount =
            await query.CountAsync(cancellationToken);

        var items =
            await query
                .Skip(request.Skip)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

        return new PagedList<T>(
            items,
            page,
            pageSize,
            totalCount);
    }
}
