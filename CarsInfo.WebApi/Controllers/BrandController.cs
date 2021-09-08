using System.Collections.Generic;
using System.Threading.Tasks;
using CarsInfo.Application.BusinessLogic.Contracts;
using CarsInfo.Application.BusinessLogic.Dtos;
using CarsInfo.WebApi.Mappers;
using CarsInfo.WebApi.ViewModels;
using CarsInfo.WebApi.ViewModels.Brand;
using Microsoft.AspNetCore.Mvc;

namespace CarsInfo.WebApi.Controllers
{
    [Route("brands")]
    public class BrandController : Controller
    {
        private readonly IBrandService _brandService;
        private readonly BrandControllerMapper _mapper;

        public BrandController(IBrandService brandService, BrandControllerMapper mapper)
        {
            _brandService = brandService;
            _mapper = mapper;
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
        public async Task<IActionResult> Create([FromBody] BrandEditorViewModel brand)
        {
            var brandDto = _mapper.MapToBrandDto(brand);
            await _brandService.AddAsync(brandDto);
            return Created("", brand);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] BrandEditorViewModel brand)
        {
            var brandDto = _mapper.MapToBrandDto(brand);
            await _brandService.UpdateAsync(brandDto);
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
