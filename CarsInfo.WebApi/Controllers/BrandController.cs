using System.Collections.Generic;
using System.Threading.Tasks;
using CarsInfo.Application.BusinessLogic.Contracts;
using CarsInfo.Application.BusinessLogic.Dtos;
using CarsInfo.Application.BusinessLogic.Enums;
using CarsInfo.WebApi.Controllers.Base;
using CarsInfo.WebApi.Mappers;
using CarsInfo.WebApi.ViewModels;
using CarsInfo.WebApi.ViewModels.Brand;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarsInfo.WebApi.Controllers
{
    [Route("brands")]
    public class BrandController : AppControllerBase
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
            
            if (operation.IsException)
            {
                return ApplicationError();
            }
            
            return operation.Success ? 
                Ok(operation.Result) :
                BadRequest(operation.FailureMessage);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<BrandDto>> Get(int id)
        {
            var operation = await _brandService.GetByIdAsync(id);
            
            if (operation.IsException)
            {
                return ApplicationError();
            }
            
            return operation.Success ?
                Ok(operation.Result) :
                BadRequest(operation.FailureMessage);
        }

        [HttpPost, Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Create([FromBody] BrandEditorViewModel brand)
        {
            var brandDto = _mapper.MapToBrandDto(brand);
            var operation = await _brandService.AddAsync(brandDto);

            if (operation.IsException)
            {
                return ApplicationError();
            }
            
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
            
            if (operation.IsException)
            {
                return ApplicationError();
            }
            
            return operation.Success ?
                NoContent() :
                BadRequest(operation.FailureMessage);
        }

        [HttpDelete("{id:int}"), Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Delete(int id)
        {
            var operation = await _brandService.DeleteByIdAsync(id);
            
            if (operation.IsException)
            {
                return ApplicationError();
            }
            
            return operation.Success ?
                NoContent() :
                BadRequest(operation.FailureMessage);
        }
    }
}
