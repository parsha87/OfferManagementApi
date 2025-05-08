using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfferManagementApi.Data.Entities;
using OfferManagementApi.Models;
using OfferManagementApi.Services;

namespace OfferManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly IBaseService<BrandDto, Brand> _brandService;

        public BrandController(IBaseService<BrandDto, Brand> brandService)
        {
            _brandService = brandService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Brand>>> GetAllBrands()
        {
            var brands = await _brandService.GetAllAsync();
            return Ok(brands);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Brand>> Get(int id)
        {
            var brand = await _brandService.GetByIdAsync(id);
            if (brand == null)
                return NotFound();

            return Ok(brand);
        }

        [HttpPost]
        public async Task<ActionResult<Brand>> Create([FromBody] BrandDto dto)
        {
            var brand = await _brandService.AddAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = brand.Id }, brand);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Brand>> Update(int id, [FromBody] BrandDto dto)
        {
            var updatedBrand = await _brandService.UpdateAsync(id, dto);
            if (updatedBrand == null)
                return NotFound();

            return Ok(updatedBrand);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBrand(int id)
        {
            var result = await _brandService.DeleteAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}
