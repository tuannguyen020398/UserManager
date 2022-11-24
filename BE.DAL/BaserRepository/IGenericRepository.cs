using BE.DAL.ModelPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BE.DAL.BaserRepository
{
    public interface IGenericRepository<TEntity, TContext> where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetAllAsync();
        ValueTask<TEntity> GetByIdAsync(params object[] keyValues);
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate, bool isTracking = false);
        Task AddAsync(TEntity entity);
        void Remove(TEntity entity);
        FilterResult<TResource> Filter<TResource>(PagingParam<TResource> pagingParams, params Expression<Func<TResource, bool>>[] predicates) where TResource : class;
        void UpdateAuditLog(TEntity entity, bool isUpdateCreated = false);
        TEntity GetById(params object[] keyValues);
        Task AddAsync<Resource>(Resource resource);

    }
}
