
using System.Linq.Expressions;


namespace Application.Common.Models
{
    public interface IGenericService<TEntity, TCreate, TUpdate, TList, TDetails>
    where TEntity : class
    where TCreate : class
    where TUpdate : class
    where TList : class
    where TDetails : class
    {
        Task<TDetails> CreateAsync(TCreate dto);
        Task UpdateAsync(TUpdate dto);
        Task DeleteAsync(Guid id);

        Task<TDetails> GetByIdAsync(Guid id, string includes = null);
        Task<List<TList>> GetAllAsync(
      Expression<Func<TEntity, bool>> filter = null,
      Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
      string includes = null);
        Task<PageResultResponseDto<TList>> GetPagedAsync(
             int page, int pageSize,
               Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
             Expression<Func<TEntity, bool>> filter = null,
             params Expression<Func<TEntity, object>>[] includes);
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> filter);

        Task<int> CountAsync(Expression<Func<TEntity, bool>> filter = null);
    }
}
