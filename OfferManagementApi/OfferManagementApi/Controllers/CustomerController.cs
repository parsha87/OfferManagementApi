using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfferManagementApi.Data.Entities;
using OfferManagementApi.Models;
using OfferManagementApi.Services;

namespace OfferManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IBaseService<CustomerDto, Customer> _apiService;
        private readonly ICustomerService _customerService;
        public CustomerController(IBaseService<CustomerDto, Customer> apiService, ICustomerService customerService)
        {
            _apiService = apiService;
            _customerService = customerService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetAllCustomers()
        {
            var customers = await _apiService.GetAllAsync();
            return Ok(customers);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> Get(int id)
        {
            var customer = await _apiService.GetByIdAsync(id);
            if (customer == null)
                return NotFound();

            return Ok(customer);
        }

        [HttpPost]
        public async Task<ActionResult<Customer>> AddCustomer([FromBody] CustomerDto dto)
        {

            var createdCustomer = await _customerService.AddCustomerAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = createdCustomer.Id }, createdCustomer);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Customer>> UpdateCustomer(int id, [FromBody] CustomerDto dto)
        {
            if (id != dto.Id)
                return BadRequest();

            var updatedCustomer = await _apiService.UpdateAsync(id, dto);
            if (updatedCustomer == null)
                return NotFound();

            return Ok(updatedCustomer);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var result = await _apiService.DeleteAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}
