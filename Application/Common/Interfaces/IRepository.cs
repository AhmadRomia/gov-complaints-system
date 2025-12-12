
using Application.Common.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;


namespace Application.Common.Interfaces
{
    public interface IRepository<T> where T : class
    {

        Task<T?> GetNoTrackingAsync(Expression<Func<T, bool>> filter = null);
        Task<IReadOnlyList<T>> GetAllAsync(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includes = null);

        Task<IReadOnlyList<T>> GetAllAsync(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            params Expression<Func<T, object>>[] includes);

        Task<IReadOnlyList<T>> GetAllAsNoTrackingAsync(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includes = null);

        Task<IReadOnlyList<T>> GetAllAsNoTrackingAsync(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            params Expression<Func<T, object>>[] includes);

        Task<PagingResult<T>> GetPagedAsync(
            int pageNumber,
            int pageSize,
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            params Expression<Func<T, object>>[] includes);

        Task<T> AddAsync(T entity);
        Task ReloadAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task UpdateRangeAsync(IEnumerable<T> entities);
        Task<IReadOnlyList<TResult>> SelectAsync<TResult>(
            Expression<Func<T, TResult>> selector,
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null);

        Task<bool> AnyAsync(Expression<Func<T, bool>> filter);

        Task<TKey> MaxAsync<TKey>(Expression<Func<T, TKey>> selector, Expression<Func<T, bool>> filter = null);
        Task<decimal> SumAsync(Expression<Func<T, decimal>> sumSelector, Expression<Func<T, bool>> filter = null);

        Task<List<TValue>> GroupByAsync<TKey, TValue>(
            Expression<Func<T, TKey>> groupBySelector,
            Expression<Func<IGrouping<TKey, T>, TValue>> projection,
            Expression<Func<T, bool>> filter = null);

        Task<int> CountAsync(Expression<Func<T, bool>> filter = null);
        Task<T> FirstOrDefaultAsync(
            Expression<Func<T, bool>> filter,
            params Expression<Func<T, object>>[] includes);

        Task<T> FirstOrDefaultAsync(
            Expression<Func<T, bool>> filter,
            string includes = null);

        Task<List<T>> AddRangeAsync(IEnumerable<T> entities);
        Task DeleteRangeAsync(IEnumerable<T> entities);
    }
}
