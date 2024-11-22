﻿using Entities.Abstract;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.DataAccess.Abstract
{
    public interface IBaseRepository<T> where T : class, IEntity, new()
    {
        Task AddAsync(T entity);
        Task AddRange(IList<T> entities);
        Task UpdateAsync(T entity);
        Task HardDeleteAsync(T entity);
        Task HardDeleteRangeAsync(IList<T> entities);
        Task SoftDeleteAsync(T entity);
        Task SoftDeleteRangeAsync(IList<T> entities);
        Task<List<T>> GetAllAsync
        (
            Expression<Func<T, bool>>? predicate = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            bool enableTracking = false
        );

        Task<List<T>> GetAllAsyncByPaging
            (
                Expression<Func<T, bool>>? predicate = null,
                Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
                Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
                bool enableTracking = false,
                int currentPage = 1, int pageSize = 3
            );
        Task<T> GetAsync
        (
            Expression<Func<T, bool>> predicate,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
            bool enableTracking = false
        );

        IQueryable<T> Find(Expression<Func<T, bool>> predicate, bool enableTracking = false);
        Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null);

        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
    }
}