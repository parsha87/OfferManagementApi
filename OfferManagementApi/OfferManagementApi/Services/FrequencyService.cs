using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OfferManagementApi.Data;
using OfferManagementApi.Data.Entities;
using OfferManagementApi.Models;

namespace OfferManagementApi.Services
{
    public interface IFrequencyService
    {
        Task<List<Frequency>> GetAllFrequenciesAsync();
        Task<Frequency?> GetFrequencyByIdAsync(int id);
        Task<Frequency> AddFrequencyAsync(Frequency frequency);
        Task<Frequency?> UpdateFrequencyAsync(Frequency frequency);
        Task<bool> DeleteFrequencyAsync(int id);
    }
    public class FrequencyService : BaseService<FrequencyDto, Frequency>, IFrequencyService
    {
        private readonly MainDBContext _context;
        private readonly IMapper _mapper;

        public FrequencyService(MainDBContext context, IMapper mapper) : base(context, mapper)  // pass both context and mapper to BaseService
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<Frequency>> GetAllFrequenciesAsync()
        {
            return await _context.Frequencies.ToListAsync();
        }

        public async Task<Frequency?> GetFrequencyByIdAsync(int id)
        {
            return await _context.Frequencies.FindAsync(id);
        }

        public async Task<Frequency> AddFrequencyAsync(Frequency frequency)
        {
            _context.Frequencies.Add(frequency);
            await _context.SaveChangesAsync();
            return frequency;
        }

        public async Task<Frequency?> UpdateFrequencyAsync(Frequency frequency)
        {
            var existingFrequency = await _context.Frequencies.FindAsync(frequency.Id);
            if (existingFrequency == null)
                return null;

            _context.Entry(existingFrequency).CurrentValues.SetValues(frequency);
            await _context.SaveChangesAsync();
            return existingFrequency;
        }

        public async Task<bool> DeleteFrequencyAsync(int id)
        {
            var frequency = await _context.Frequencies.FindAsync(id);
            if (frequency == null)
                return false;

            _context.Frequencies.Remove(frequency);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
