using System.Threading.Tasks;
using CarsInfo.Application.BusinessLogic.Contracts;
using CarsInfo.Application.BusinessLogic.Dtos;
using CarsInfo.Application.BusinessLogic.Enums;
using CarsInfo.WebApi.Mappers;
using CarsInfo.WebApi.ViewModels.Brand;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarsInfo.WebApi.Controllers
{
    [Route("brands")]
    public class BrandController : ControllerBase
    {
        private readonly IBrandService _brandService;
        private readonly BrandControllerMapper _mapper;

        public BrandController(IBrandService brandService, BrandControllerMapper mapper)
        {
            _brandService = brandService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string name)
        {
            var operation = await _brandService.GetAllAsync(name);
            
            return operation.Success ? 
                Ok(operation.Result) :
                BadRequest(operation.FailureMessage);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<BrandDto>> Get(int id)
        {
            var operation = await _brandService.GetByIdAsync(id);

            if (!operation.Success)
            {
                return BadRequest(operation.FailureMessage);
            }

            return operation.Result is null ?
                NotFound() :
                Ok(operation.Result);
        }

        [HttpPost, Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Create([FromBody] BrandEditorViewModel brand)
        {
            var brandDto = _mapper.MapToBrandDto(brand);
            var operation = await _brandService.AddAsync(brandDto);
            
            return operation.Success ?
                CreatedAtAction(nameof(Get), new { id = operation.Result }, brand) :
                BadRequest(operation.FailureMessage);
        }

        [HttpPut("{id:int}"), Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Update(int id, [FromBody] BrandEditorViewModel brand)
        {
            var brandDto = _mapper.MapToBrandDto(brand);
            brandDto.Id = id;
            var operation = await _brandService.UpdateAsync(brandDto);
            
            return operation.Success ?
                NoContent() :
                BadRequest(operation.FailureMessage);
        }

        [HttpDelete("{id:int}"), Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Delete(int id)
        {
            var operation = await _brandService.DeleteByIdAsync(id);
            
            return operation.Success ?
                NoContent() :
                BadRequest(operation.FailureMessage);
        }
    }
}
