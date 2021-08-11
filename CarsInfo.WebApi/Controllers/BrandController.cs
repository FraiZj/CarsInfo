using System.Collections.Generic;
using System.Threading.Tasks;
using CarsInfo.BLL.Contracts;
using CarsInfo.BLL.Models.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace CarsInfo.WebApi.Controllers
{
    [Route("brands")]
    public class BrandController : Controller
    {
        private readonly IBrandService _brandService;

        public BrandController(IBrandService brandService)
        {
            _brandService = brandService;
        }

        [HttpGet]
        public async Task<IEnumerable<BrandDto>> Get(string name)
        {
            var brands = await _brandService.GetAllAsync(name);
            return brands;
        }

        [HttpGet("{id:int}")]
        public async Task<BrandDto> Get(int id)
        {
            var brand = await _brandService.GetByIdAsync(id);
            return brand;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BrandDto brand)
        {
            await _brandService.AddAsync(brand);
            return Created("", brand);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] BrandDto brand)
        {
            await _brandService.UpdateAsync(brand);
            return Ok(brand);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _brandService.DeleteByIdAsync(id);
            return Ok($"Brand with id={id} has been deleted");
        }
    }
}
