using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfferManagementApi.Data.Entities;
using OfferManagementApi.Models;
using OfferManagementApi.Services;

namespace OfferManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ListOfValuesController : ControllerBase
    {
        private readonly IBaseService<ListOfValueDto, ListOfValue> _apiService;
        private readonly IListOfValueService _listOfValueService;
        public ListOfValuesController(IBaseService<ListOfValueDto, ListOfValue> apiService, IListOfValueService listOfValueService)
        {
            _apiService = apiService;
            _listOfValueService = listOfValueService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ListOfValue>>> GetListOfValues()
        {
            var listOfValues = await _apiService.GetAllAsync();
            return Ok(listOfValues);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ListOfValue>> GetListOfValue(int id)
        {
            var listOfValue = await _apiService.GetByIdAsync(id);
            if (listOfValue == null)
            {
                return NotFound();
            }
            return Ok(listOfValue);
        }

        [HttpPost]
        public async Task<ActionResult<ListOfValue>> CreateListOfValue([FromBody] ListOfValueDto dto)
        {
            var createdListOfValue = await _listOfValueService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetListOfValue), new { id = createdListOfValue.Id }, createdListOfValue);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ListOfValue>> UpdateListOfValue(int id, [FromBody]ListOfValueDto dto)
        {
            if (id != dto.Id)
                return BadRequest();

            var updatedListOfValue = await _apiService.UpdateAsync(id, dto);
            if (updatedListOfValue == null)
            {
                return NotFound();
            }
            return Ok(updatedListOfValue);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteListOfValue(int id)
        {
            var success = await _apiService.DeleteAsync(id);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
