using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using OfferManagementApi.Data;
using OfferManagementApi.Data.Entities;
using OfferManagementApi.Models;

namespace OfferManagementApi.Services
{
    public interface IBrandService
    {
        Task<List<Brand>> GetAllBrandsAsync();
        Task<Brand?> GetBrandByIdAsync(int id);
        Task<Brand> AddBrandAsync(Brand brand);
        Task<Brand?> UpdateBrandAsync(Brand brand);
        Task<bool> DeleteBrandAsync(int id);
        Task<(List<BrandDto> Data, int TotalCount)> GetPagedBrandsAsync(int pageNumber, int pageSize);
    }
    public class BrandService :  BaseService<BrandDto, Brand>, IBrandService
    {
        private readonly MainDBContext _context;
        private readonly IMapper _mapper;

        public BrandService(MainDBContext context, IMapper mapper)
         : base(context, mapper)  // pass both context and mapper to BaseService
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<(List<BrandDto> Data, int TotalCount)> GetPagedBrandsAsync(int pageNumber, int pageSize)
        {
            var query = _context.Brands.Where(b => b.IsActive);

            var totalCount = await query.CountAsync(); // 🔥 Get total before pagination

            var data = await query
                .OrderBy(b => b.BrandName)
                .ProjectTo<BrandDto>(_mapper.ConfigurationProvider)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (data, totalCount);
        }

        public async Task<List<Brand>> GetAllBrandsAsync()
        {
            return await _context.Brands.ToListAsync();
        }

        public async Task<Brand?> GetBrandByIdAsync(int id)
        {
            return await _context.Brands.FindAsync(id);
        }

        public async Task<Brand> AddBrandAsync(Brand brand)
        {
            _context.Brands.Add(brand);
            await _context.SaveChangesAsync();
            return brand;
        }

        public async Task<Brand?> UpdateBrandAsync(Brand brand)
        {
            var existingBrand = await _context.Brands.FindAsync(brand.Id);
            if (existingBrand == null)
                return null;

            _context.Entry(existingBrand).CurrentValues.SetValues(brand);
            await _context.SaveChangesAsync();
            return existingBrand;
        }

        public async Task<bool> DeleteBrandAsync(int id)
        {
            var brand = await _context.Brands.FindAsync(id);
            if (brand == null)
                return false;

            _context.Brands.Remove(brand);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
