using Application.Common.Interfaces;
using Application.Common.Models;
using Infrastructure.Extentions;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class GenericRepository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<T> _dbSet;
        private const int DefaultConcurrencyRetries = 3;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = _context.Set<T>();
        }

        #region Add / Update / Delete / Reload

        public virtual async Task<T> AddAsync(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            _dbSet.Add(entity);
            await SaveChangesWithRetryAsync();
            await _context.Entry(entity).ReloadAsync();
            return entity;
        }

        public virtual async Task<List<T>> AddRangeAsync(IEnumerable<T> entities)
        {
            var list = entities.ToList();
            if (!list.Any()) return list;
            _dbSet.AddRange(list);
            await SaveChangesWithRetryAsync();
            foreach (var e in list)
                await _context.Entry(e).ReloadAsync();
            return list;
        }

        public virtual async Task ReloadAsync(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            await _context.Entry(entity).ReloadAsync();
        }

        public virtual async Task UpdateAsync(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            _dbSet.Update(entity);
            await SaveChangesWithRetryAsync();
            await _context.Entry(entity).ReloadAsync();
        }

        public virtual async Task UpdateRangeAsync(IEnumerable<T> entities)
        {
            var list = entities.ToList();
            if (!list.Any()) return;
            _dbSet.UpdateRange(list);
            await SaveChangesWithRetryAsync();
            foreach (var e in list)
                await _context.Entry(e).ReloadAsync();
        }

        public virtual async Task DeleteAsync(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            _dbSet.Remove(entity);
            await SaveChangesWithRetryAsync();
        }

        public virtual async Task DeleteRangeAsync(IEnumerable<T> entities)
        {
            var list = entities.ToList();
            if (!list.Any()) return;
            _dbSet.RemoveRange(list);
            await SaveChangesWithRetryAsync();
        }

        #endregion

        #region Get / Query / FirstOrDefault / Any / Count / Paging

        public async Task<IReadOnlyList<T>> GetAllAsync(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includes = null)
        {
            IQueryable<T> query = _dbSet.AsQueryable();
            if (!string.IsNullOrWhiteSpace(includes))
            {
                foreach (var include in includes.Split(',', StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(include.Trim());
                }
            }
            if (filter != null) query = query.Where(filter);
            if (orderBy != null) query = orderBy(query);
            return await query.ToListAsync();
        }

        public async Task<IReadOnlyList<T>> GetAllAsync(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet.AsQueryable();
            query = IncludeLambda(query, includes);
            if (filter != null) query = query.Where(filter);
            if (orderBy != null) query = orderBy(query);
            return await query.ToListAsync();
        }

        public async Task<IReadOnlyList<T>> GetAllAsNoTrackingAsync(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includes = null)
        {
            IQueryable<T> query = _dbSet.AsNoTracking();
            if (!string.IsNullOrWhiteSpace(includes))
            {
                foreach (var include in includes.Split(',', StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(include.Trim());
                }
            }
            if (filter != null) query = query.Where(filter);
            if (orderBy != null) query = orderBy(query);
            return await query.ToListAsync();
        }

        public async Task<IReadOnlyList<T>> GetAllAsNoTrackingAsync(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet.AsNoTracking();
            query = IncludeLambda(query, includes);
            if (filter != null) query = query.Where(filter);
            if (orderBy != null) query = orderBy(query);
            return await query.ToListAsync();
        }

        public async Task<PagingResult<T>> GetPagedAsync(int pageNumber, int pageSize,
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            params Expression<Func<T, object>>[] includes)
        {
            if (pageNumber <= 0) pageNumber = 1;
            if (pageSize <= 0) pageSize = 10;

            IQueryable<T> query = _dbSet.AsNoTracking();
            query = IncludeLambda(query, includes);
            if (filter != null) query = query.Where(filter);
            if (orderBy != null) query = orderBy(query);

            var count = await query.CountAsync();
            var data = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PagingResult<T> { Data = data, Count = count };
        }

        public async Task<T> FirstOrDefaultAsync(
            Expression<Func<T, bool>> filter,
            params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet;
            query = IncludeLambda(query, includes);
            return await query.FirstOrDefaultAsync(filter);
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> filter, string includes = null)
        {
            IQueryable<T> query = _dbSet;
            if (!string.IsNullOrWhiteSpace(includes))
            {
                foreach (var include in includes.Split(',', StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(include.Trim());
                }
            }
            return await query.AsNoTracking().FirstOrDefaultAsync(filter);
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> filter)
        {
            IQueryable<T> query = _dbSet;
            if (filter != null) query = query.Where(filter);
            return await query.AnyAsync();
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>> filter = null)
        {
            IQueryable<T> query = _dbSet;
            if (filter != null) query = query.Where(filter);
            return await query.CountAsync();
        }

        public async Task<T?> GetByIdAsNoTrackingAsync(Expression<Func<T, bool>> filter = null)
        {
            return await _dbSet.AsNoTracking().FirstOrDefaultAsync(filter);
        }


        #endregion

        #region Aggregations / Select / GroupBy / Sum / Max

        public async Task<IReadOnlyList<TResult>> SelectAsync<TResult>(
            Expression<Func<T, TResult>> selector,
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null)
        {
            IQueryable<T> query = _dbSet;
            if (filter != null) query = query.Where(filter);
            if (orderBy != null) query = orderBy(query);
            return await query.Select(selector).ToListAsync();
        }

        public async Task<TKey> MaxAsync<TKey>(Expression<Func<T, TKey>> selector, Expression<Func<T, bool>> filter = null)
        {
            IQueryable<T> query = _dbSet;
            if (filter != null) query = query.Where(filter);

            if (!await query.AnyAsync()) return default;
            return await query.MaxAsync(selector);
        }

        public async Task<decimal> SumAsync(Expression<Func<T, decimal>> sumSelector, Expression<Func<T, bool>> filter = null)
        {
            IQueryable<T> query = _dbSet;
            if (filter != null) query = query.Where(filter);
            return await query.SumAsync(sumSelector);
        }

        public async Task<List<TValue>> GroupByAsync<TKey, TValue>(
            Expression<Func<T, TKey>> groupBySelector,
            Expression<Func<IGrouping<TKey, T>, TValue>> projection,
            Expression<Func<T, bool>> filter = null)
        {
            IQueryable<T> query = _dbSet;
            if (filter != null) query = query.Where(filter);
            return await query.GroupBy(groupBySelector).Select(projection).ToListAsync();
        }

        #endregion

        #region Helpers

        private static IQueryable<T> IncludeLambda(IQueryable<T> query, params Expression<Func<T, object>>[] propertySelectors)
        {
            if (propertySelectors != null && propertySelectors.Length > 0)
            {
                foreach (var sel in propertySelectors)
                {
                    var path = sel.AsPath();
                    if (!string.IsNullOrWhiteSpace(path))
                        query = query.Include(path);
                }
            }
            return query;
        }

        private async Task SaveChangesWithRetryAsync(int maxRetries = DefaultConcurrencyRetries)
        {
            var attempts = 0;
            while (true)
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return;
                }
                catch (DbUpdateConcurrencyException)
                {
                    attempts++;
                    if (attempts >= maxRetries) throw;
                    await Task.Delay(50 * attempts);
                }
            }
        }

        #endregion
    }
}
