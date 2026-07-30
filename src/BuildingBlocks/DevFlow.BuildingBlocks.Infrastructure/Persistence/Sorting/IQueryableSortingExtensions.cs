using DevFlow.SharedKernel.Pagination;
using System.Linq.Expressions;

namespace DevFlow.BuildingBlocks.Infrastructure.Persistence.Sorting;

public static class IQueryableSortingExtensions
{
    public static IQueryable<TEntity> ApplySorting<TEntity>(
        this IQueryable<TEntity> query,
        PaginationRequest pagination,
        Dictionary<string, Expression<Func<TEntity, object>>> mappings)
    {
        if (string.IsNullOrWhiteSpace(pagination.SortBy))
            return query;

        if (!mappings.TryGetValue(
                pagination.SortBy,
                out var expression))
        {
            return query;
        }

        return pagination.IsDescending
            ? query.OrderByDescending(expression)
            : query.OrderBy(expression);
    }
}
