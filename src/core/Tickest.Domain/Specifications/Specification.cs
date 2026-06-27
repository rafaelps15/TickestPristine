using System.Linq.Expressions;
using Tickest.SharedKernel;

namespace Tickest.Domain.Specifications;

public abstract class Specification<TEntity, TEntityId>
    where TEntity : Entity<TEntityId>
    where TEntityId : ValueObject
{
    protected Specification(Expression<Func<TEntity, bool>> criteria)
    {
        Criteria = criteria;
    }

    public Expression<Func<TEntity, bool>> Criteria { get; }

    public List<Expression<Func<TEntity, object>>> Includes { get; } = [];

    protected void AddInclude(Expression<Func<TEntity, object>> include)
    {
        Includes.Add(include);
    }
}
