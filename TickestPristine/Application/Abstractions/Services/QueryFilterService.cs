using Microsoft.EntityFrameworkCore;
using System.Linq;
using Tickest.Application.Abstractions.Services;

namespace Tickest.Application.Services
{
    public class QueryFilterService : IQueryFilterService
    {
        public IQueryable<TEntity> ApplyFilters<TEntity>(IQueryable<TEntity> query) where TEntity : class
        {
            return query.Where(entity => EF.Property<bool>(entity, "IsActive"));
        }
    }
}
