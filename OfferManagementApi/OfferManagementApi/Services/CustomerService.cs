using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OfferManagementApi.Data;
using OfferManagementApi.Data.Entities;
using OfferManagementApi.Models;

namespace OfferManagementApi.Services
{
    public interface ICustomerService
    {
        Task<List<Customer>> GetAllCustomersAsync();
        Task<Customer?> GetCustomerByIdAsync(int id);
        Task<Customer> AddCustomerAsync(CustomerDto customer);
        Task<Customer?> UpdateCustomerAsync(Customer customer);
        Task<bool> DeleteCustomerAsync(int id);
    }
    public class CustomerService : BaseService<CustomerDto, Customer>, ICustomerService
    {
        private readonly MainDBContext _context;
        private readonly IMapper _mapper;
        private readonly IBaseService<CustomerDto, Customer> _apiService;

        public CustomerService(MainDBContext context, IMapper mapper)
         : base(context, mapper)  // pass both context and mapper to BaseService
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<Customer>> GetAllCustomersAsync()
        {
            return await _context.Customers.ToListAsync();
        }

        public async Task<Customer?> GetCustomerByIdAsync(int id)
        {
            return await _context.Customers.FindAsync(id);
        }

        public async Task<Customer> AddCustomerAsync(CustomerDto dto)
        {
            var trimmedName = dto.CustomerName.Trim();

            var existingCustomer = await _context.Customers
                .FirstOrDefaultAsync(c => c.CustomerName.Trim().ToLower() == trimmedName.ToLower());

            if (existingCustomer != null)
            {
                throw new Exception("Customer with the same name already exists.");
            }

            // If no duplicate, then call base AddAsync
            return await base.AddAsync(dto);
        }

        public async Task<Customer?> UpdateCustomerAsync(Customer customer)
        {
            var existingCustomer = await _context.Customers.FindAsync(customer.Id);
            if (existingCustomer == null)
                return null;

            _context.Entry(existingCustomer).CurrentValues.SetValues(customer);
            await _context.SaveChangesAsync();
            return existingCustomer;
        }

        public async Task<bool> DeleteCustomerAsync(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
                return false;

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}