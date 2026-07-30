namespace DevFlow.BuildingBlocks.Infrastructure.Persistence.Sorting;

public interface ISorting<TEntity>
{
    IQueryable<TEntity> Apply(
        IQueryable<TEntity> query,
        string? sortBy,
        bool descending);
}
