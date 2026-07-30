using System.Linq.Expressions;

namespace DevFlow.BuildingBlocks.Infrastructure.Persistence.Sorting;

public abstract class Sorting<TEntity>
    : ISorting<TEntity>
{
    private readonly Dictionary<string, LambdaExpression> _mappings =
        new(StringComparer.OrdinalIgnoreCase);

    protected void Map<TKey>(
        string name,
        Expression<Func<TEntity, TKey>> expression)
    {
        _mappings[name] = expression;
    }

    public IQueryable<TEntity> Apply(
        IQueryable<TEntity> query,
        string? sortBy,
        bool descending)
    {
        if (string.IsNullOrWhiteSpace(sortBy))
            return query;

        if (!_mappings.TryGetValue(sortBy, out var lambda))
            return query;

        return ApplyOrdering(
            query,
            lambda,
            descending);
    }

    private static IQueryable<TEntity> ApplyOrdering(
        IQueryable<TEntity> query,
        LambdaExpression lambda,
        bool descending)
    {
        var method = descending
            ? nameof(Queryable.OrderByDescending)
            : nameof(Queryable.OrderBy);

        var result =
            typeof(Queryable)
                .GetMethods()
                .Single(x =>
                    x.Name == method &&
                    x.GetParameters().Length == 2)
                .MakeGenericMethod(
                    typeof(TEntity),
                    lambda.ReturnType)
                .Invoke(
                    null,
                    new object[]
                    {
                        query,
                        lambda
                    });

        return (IQueryable<TEntity>)result!;
    }
}
