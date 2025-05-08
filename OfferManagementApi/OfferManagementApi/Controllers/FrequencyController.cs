using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfferManagementApi.Data.Entities;
using OfferManagementApi.Models;
using OfferManagementApi.Services;

namespace OfferManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FrequencyController : ControllerBase
    {
        private readonly IBaseService<FrequencyDto, Frequency> _apiService;

        public FrequencyController(IBaseService<FrequencyDto, Frequency> apiService)
        {
            _apiService = apiService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Frequency>>> GetAllFrequencies()
        {
            var frequencies = await _apiService.GetAllAsync();
            return Ok(frequencies);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Frequency>> GetFrequencyById(int id)
        {
            var frequency = await _apiService.GetByIdAsync(id);
            if (frequency == null)
                return NotFound();

            return Ok(frequency);
        }

        [HttpPost]
        public async Task<ActionResult<Frequency>> AddFrequency([FromBody] FrequencyDto dto)
        {

            var addObj = await _apiService.AddAsync(dto);
            return CreatedAtAction(nameof(GetAllFrequencies), new { id = addObj.Id }, addObj);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Frequency>> UpdateFrequency(int id, FrequencyDto dto)
        {
            if (id != dto.Id)
                return BadRequest();

            var updatedBrand = await _apiService.UpdateAsync(id, dto);
            if (updatedBrand == null)
                return NotFound();

            return Ok(updatedBrand);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFrequency(int id)
        {
            var result = await _apiService.DeleteAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}
