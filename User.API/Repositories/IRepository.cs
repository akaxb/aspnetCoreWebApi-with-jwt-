using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace User.API.Repositories
{
    public interface IRepository<TEntity, TKey>
         where TEntity : class
    {
        Task<TKey> CreateAsync(TEntity entity);

        Task UpdateAsync(TEntity entity);

        Task DeleteAsync(TKey id);

        Task<TEntity> FindByIdAsync(TKey id);

        Task<TEntity> SinleAsync(Expression<Func<TEntity, bool>> expression = null);

        IQueryable<TEntity> GetAllList(Expression<Func<TEntity, bool>> predicate = null);

        Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> expression=null);
    }
}
