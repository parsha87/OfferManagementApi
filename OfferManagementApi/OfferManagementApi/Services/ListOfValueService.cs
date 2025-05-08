using OfferManagementApi.Data.Entities;
using OfferManagementApi.Data;
using Microsoft.EntityFrameworkCore;
using OfferManagementApi.Models;
using AutoMapper;

namespace OfferManagementApi.Services
{
    public interface IListOfValueService
    {
        Task<List<ListOfValue>> GetAllAsync();
        Task<ListOfValue> GetByIdAsync(int id);
        Task<ListOfValue> CreateAsync(ListOfValueDto listOfValue);
        Task<ListOfValue> UpdateAsync(int id, ListOfValue listOfValue);
        Task<bool> DeleteAsync(int id);
    }
    public class ListOfValueService : BaseService<ListOfValueDto, ListOfValue>, IListOfValueService
    {
        private readonly MainDBContext _context;
        private readonly IMapper _mapper;
        private readonly IBaseService<CustomerDto, Customer> _apiService;

        public ListOfValueService(MainDBContext context, IMapper mapper)
         : base(context, mapper)  // pass both context and mapper to BaseService
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<ListOfValue>> GetAllAsync()
        {
            return await _context.ListOfValues.ToListAsync();
        }

        public async Task<ListOfValue> GetByIdAsync(int id)
        {
            return await _context.ListOfValues.FindAsync(id);
        }

        public async Task<ListOfValue> CreateAsync(ListOfValueDto dto)
        {
            var trimmedName = dto.Value.Trim();

            var existingCustomer = await _context.ListOfValues
                .FirstOrDefaultAsync(c => c.Value.Trim().ToLower() == trimmedName.ToLower() && c.Type.Trim() == dto.Type.Trim());

            if (existingCustomer != null)
            {
                throw new Exception("Value exist for type" + dto.Type);
            }

            // If no duplicate, then call base AddAsync
            return await base.AddAsync(dto);
        }

        public async Task<ListOfValue> UpdateAsync(int id, ListOfValue listOfValue)
        {
            var existing = await _context.ListOfValues.FindAsync(id);
            if (existing != null)
            {
                existing.Value = listOfValue.Value;
                existing.Type = listOfValue.Type;

                await _context.SaveChangesAsync();
            }

            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var listOfValue = await _context.ListOfValues.FindAsync(id);
            if (listOfValue != null)
            {
                _context.ListOfValues.Remove(listOfValue);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }
    }
}
