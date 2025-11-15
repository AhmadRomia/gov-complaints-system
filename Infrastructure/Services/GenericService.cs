using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;


namespace Infrastructure.Services
{
    public class GenericService<TEntity, TCreate, TUpdate, TList, TDetails>
    : IGenericService<TEntity, TCreate, TUpdate, TList, TDetails>
    where TEntity : class
    where TCreate : class
    where TUpdate : class
    where TList : class
    where TDetails : class
    {
        protected readonly IRepository<TEntity> _repository;
        protected readonly IMapper _mapper;

        public GenericService(IRepository<TEntity> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<TDetails> CreateAsync(TCreate dto)
        {
            var entity = _mapper.Map<TEntity>(dto);
            var result = await _repository.AddAsync(entity);
            return _mapper.Map<TDetails>(result);
        }

        public async Task UpdateAsync(TUpdate dto)
        {
            var entity = _mapper.Map<TEntity>(dto);
            await _repository.UpdateAsync(entity);
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _repository.FirstOrDefaultAsync(x => EF.Property<Guid>(x, "Id") == id);
            await _repository.DeleteAsync(entity);
        }

        public async Task<TDetails> GetByIdAsync(Guid id)
        {
            var entity = await _repository.FirstOrDefaultAsync(x => EF.Property<Guid>(x, "Id") == id);
            return _mapper.Map<TDetails>(entity);
        }

        public async Task<List<TList>> GetAllAsync(Expression<Func<TEntity, bool>> filter = null)
        {
            var data = await _repository.GetAllAsNoTrackingAsync(filter);
            return _mapper.Map<List<TList>>(data);
        }

        public async Task<PagingResult<TList>> GetPagedAsync(
            int page, int pageSize,
            Expression<Func<TEntity, bool>> filter = null)
        {
            var result = await _repository.GetPagedAsync(page, pageSize, filter);
            return new PagingResult<TList>
            {
                Count = result.Count,
                Data = _mapper.Map<List<TList>>(result.Data)
            };
        }

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> filter)
        {
            return await _repository.AnyAsync(filter);
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> filter = null)
        {
            return await _repository.CountAsync(filter);
        }
    }
}
