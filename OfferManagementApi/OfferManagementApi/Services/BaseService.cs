using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using OfferManagementApi.Data;

namespace OfferManagementApi.Services
{
    public interface IBaseService<TDto, TEntity>
    {
        Task<List<TDto>> GetAllAsync();
        Task<TEntity?> GetByIdAsync(int id);
        Task<TEntity> AddAsync(TDto dto);
        Task<TEntity?> UpdateAsync(int id, TDto dto);
        Task<bool> DeleteAsync(int id);
    }
    public class BaseService<TDto, TEntity> : IBaseService<TDto, TEntity>
     where TEntity : class, new()
    {
        private readonly MainDBContext _context;
        private readonly IMapper _mapper;

        public BaseService(MainDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<TDto>> GetAllAsync()
        {
            return await _context.Set<TEntity>()
                .ProjectTo<TDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<TEntity?> GetByIdAsync(int id)
        {
            return await _context.Set<TEntity>().FindAsync(id);
        }

        public async Task<TEntity> AddAsync(TDto dto)
        {
            var entity = _mapper.Map<TEntity>(dto);
            _context.Set<TEntity>().Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<TEntity?> UpdateAsync(int id, TDto dto)
        {
            var entity = await _context.Set<TEntity>().FindAsync(id);
            if (entity == null)
                return null;

            _mapper.Map(dto, entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.Set<TEntity>().FindAsync(id);
            if (entity == null)
                return false;

            _context.Set<TEntity>().Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
