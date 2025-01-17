namespace Tickest.Application.Abstractions.Services;

public interface IQueryFilterService
{
    IQueryable<TEntity> ApplyFilters<TEntity>(IQueryable<TEntity> query) where TEntity : class;

}
