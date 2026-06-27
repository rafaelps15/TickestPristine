using Microsoft.EntityFrameworkCore;
using Tickest.Domain.Specifications;
using Tickest.SharedKernel;

namespace Tickest.Persistence.Repositories;

internal static class SpecificationEvaluator
{
    public static IQueryable<TEntity> GetQuery<TEntity, TEntityId>(
        IQueryable<TEntity> query,
        Specification<TEntity, TEntityId> specification)
        where TEntity : Entity<TEntityId>
        where TEntityId : ValueObject
    {
        var specificationQuery = query.Where(specification.Criteria);

        return specification.Includes.Aggregate(
            specificationQuery,
            (current, include) => current.Include(include));
    }
}
