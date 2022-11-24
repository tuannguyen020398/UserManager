﻿using AutoMapper;
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
    public class GenericRepository<TEntity, TContext> : IGenericRepository<TEntity, TContext> where TEntity : class where TContext : DbContext
    {

        protected readonly TContext Context;

        private readonly DbSet<TEntity> _dbSet;
        private readonly long _userId;
        private readonly IMapper _mapper;
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
        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

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
        public void Remove(TEntity entity)
        {
            _dbSet.Remove(entity);

        }
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
