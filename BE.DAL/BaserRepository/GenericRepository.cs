using AutoMapper;
using BE.DAL.ModelPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using AutoMapper.QueryableExtensions;
using BE.DAL.Utility;

namespace BE.DAL.BaserRepository
{
    /// <summary>phương thức khởi tạo lớp base reponsitory</summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <typeparam name="TContext">The type of the context.</typeparam>
    /// <Modified>
    /// Name Date Comments
    /// tuannx 12/1/2022 created
    /// </Modified>
    public class GenericRepository<TEntity, TContext> : IGenericRepository<TEntity, TContext> where TEntity : class where TContext : DbContext
    {

        protected readonly TContext Context;

        private readonly DbSet<TEntity> _dbSet;
        private readonly long _userId;
        private readonly IMapper _mapper;
        /// <summary>Gets the by identifier.</summary>
        /// <param name="keyValues">The key values.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <Modified>
        /// Name Date Comments
        /// tuannx 12/1/2022 created
        /// </Modified>
        public TEntity GetById(params object[] keyValues)
        {
            return _dbSet.Find(keyValues);
        }

        public GenericRepository(TContext context, IMapper mapper)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", isEnabled: true);
            Context = context;
            _dbSet = context.Set<TEntity>();
            //if (httpContextAccessor != null && httpContextAccessor.HttpContext != null)
            //{
            //    _userId = ConvertUtility.ConvertToLong(ClaimHelpers.GetUserId(httpContextAccessor.HttpContext!.User));
            //}
            _mapper = mapper;
        }
        /// <summary>phương thức lấy ra danh sách</summary>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <Modified>
        /// Name Date Comments
        /// tuannx 12/1/2022 created
        /// </Modified>
        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        /// <summary>Filters danh sách theo thuộc  tính</summary>
        /// <typeparam name="TResource">The type of the resource.</typeparam>
        /// <param name="pagingParams">The paging parameters.</param>
        /// <param name="predicates">The predicates.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <exception cref="System.ArgumentNullException">pagingParams
        /// or
        /// SortExpression require</exception>
        /// <Modified>
        /// Name Date Comments
        /// tuannx 12/1/2022 created
        /// </Modified>
        public FilterResult<TResource> Filter<TResource>(PagingParam<TResource> pagingParams, params Expression<Func<TResource, bool>>[] predicates) where TResource : class
        {
            int pageIndex = pagingParams.PageIndex;
            int pageSize = pagingParams.PageSize;
            bool flag = pageIndex > 0 && pageSize > 0;
            if (pagingParams == null)
            {
                throw new ArgumentNullException("pagingParams");
            }

            FilterResult<TResource> filterResult = new FilterResult<TResource>();
            IQueryable<TResource> source = Context.Set<TEntity>().ProjectTo(_mapper.ConfigurationProvider, Array.Empty<Expression<Func<TResource, object>>>());
            List<Expression<Func<TResource, bool>>> predicates2 = pagingParams.GetPredicates();
            if (predicates != null)
            {
                predicates2.AddRange(predicates.ToList());
                predicates = predicates2.ToArray();
            }

            Expression<Func<TResource, bool>>[] array = predicates;
            foreach (Expression<Func<TResource, bool>> predicate in array)
            {
                source = source.Where(predicate);
            }

            if (flag)
            {
                filterResult.TotalRows = source.Count();
            }

            if (!string.IsNullOrEmpty(pagingParams.SortExpression))
            {
                source = source.OrderBy(pagingParams.SortExpression);
            }
            else if (typeof(TEntity).GetProperty("Id") != null)
            {
                source = source.OrderBy("Id desc");
            }
            else if (flag)
            {
                throw new ArgumentNullException("SortExpression require");
            }

            if (flag)
            {
                source = source.Skip((pageIndex - 1) * pageSize + pagingParams.Skip).Take(pageSize);
            }

            filterResult.Data = source.ToList();
            return filterResult;
        }
        /// <summary>Adds the asynchronous.</summary>
        /// <typeparam name="TResource">The type of the resource.</typeparam>
        /// <param name="resource">The resource.</param>
        /// <Modified>
        /// Name Date Comments
        /// tuannx 12/1/2022 created
        /// </Modified>
        public async Task AddAsync<TResource>(TResource resource)
        {
            TEntity entity = _mapper.Map<TResource, TEntity>(resource);
            UpdateAuditLog(entity, isUpdateCreated: true);
            await _dbSet.AddAsync(entity);
        }
        public async Task AddAsync(TEntity entity)
        {
            Context.Entry(entity).State = EntityState.Added;
            UpdateAuditLog(entity, isUpdateCreated: true);
            await _dbSet.AddAsync(entity);

            Context.SaveChanges();
        }
        /// <summary>Gets the by identifier asynchronous.</summary>
        /// <param name="keyValues">The key values.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <Modified>
        /// Name Date Comments
        /// tuannx 12/1/2022 created
        /// </Modified>
        public ValueTask<TEntity> GetByIdAsync(params object[] keyValues)
        {
            return _dbSet.FindAsync(keyValues);
        }
        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate, bool isTracking = false)
        {
            if (isTracking)
            {
                return _dbSet.Where(predicate);
            }

            return EntityFrameworkQueryableExtensions.AsNoTracking(_dbSet.Where(predicate));
        }
        /// <summary>Removes the specified entity.</summary>
        /// <param name="entity">The entity.</param>
        /// <Modified>
        /// Name Date Comments
        /// tuannx 12/1/2022 created
        /// </Modified>
        public void Remove(TEntity entity)
        {
            _dbSet.Remove(entity);

        }
        /// <summary>Updates the audit log.</summary>
        /// <param name="entity">The entity.</param>
        /// <param name="isUpdateCreated">if set to <c>true</c> [is update created].</param>
        /// <Modified>
        /// Name Date Comments
        /// tuannx 12/1/2022 created
        /// </Modified>
        public void UpdateAuditLog(TEntity entity, bool isUpdateCreated = false)
        {
            if (isUpdateCreated)
            {
                if (ConvertUtility.GetObjectPropertyValueAsObject(entity, "CreatedBy") != null)
                {
                    ConvertUtility.SetObjectPropertyValue(entity, "CreatedBy", CurrentUser.Id);
                }

                if (ConvertUtility.GetObjectPropertyValueAsObject(entity, "CreatedDate") != null)
                {
                    ConvertUtility.SetObjectPropertyValue(entity, "CreatedDate", CurrentUser.UtcNow);
                }
            }

            if (ConvertUtility.GetObjectPropertyValueAsObject(entity, "LastModifiedBy") != null)
            {
                ConvertUtility.SetObjectPropertyValue(entity, "LastModifiedBy", CurrentUser.Id);
            }

            if (ConvertUtility.GetObjectPropertyValueAsObject(entity, "LastModifiedDate") != null)
            {
                ConvertUtility.SetObjectPropertyValue(entity, "LastModifiedDate", CurrentUser.UtcNow);
            }

            if (ConvertUtility.GetObjectPropertyValueAsObject(entity, "CreatedDate") != null)
            {
                DateTime value = (DateTime)ConvertUtility.GetObjectPropertyValueAsObject(entity, "CreatedDate");
                if (value.Kind != 0)
                {
                    value = DateTime.SpecifyKind(value, DateTimeKind.Unspecified);
                    ConvertUtility.SetObjectPropertyValue(entity, "CreatedDate", value);
                }
            }

            if (ConvertUtility.GetObjectPropertyValueAsObject(entity, "LastModifiedDate") != null)
            {
                DateTime value2 = (DateTime)ConvertUtility.GetObjectPropertyValueAsObject(entity, "LastModifiedDate");
                if (value2.Kind != 0)
                {
                    value2 = DateTime.SpecifyKind(value2, DateTimeKind.Unspecified);
                    ConvertUtility.SetObjectPropertyValue(entity, "LastModifiedDate", value2);
                }
            }
        }
    }
}
