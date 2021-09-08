using System.Linq;
using System.Threading.Tasks;
using CarsInfo.Application.BusinessLogic.Contracts;
using CarsInfo.Application.BusinessLogic.Dtos;
using CarsInfo.Application.BusinessLogic.Enums;
using CarsInfo.WebApi.Extensions;
using CarsInfo.WebApi.Mappers;
using CarsInfo.WebApi.ViewModels.Car;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CarsInfo.WebApi.Controllers
{
    [Route("cars")]
    public class CarsController : ControllerBase
    {
        private readonly ICarsService _carsService;
        private readonly CarsControllerMapper _mapper;

        public CarsController(
            ICarsService carsService,
            CarsControllerMapper mapper)
        {
            _carsService = carsService;
            _mapper = mapper;
        }
        
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Get([FromQuery] FilterDto filter)
        {
            var cars = await _carsService.GetAllAsync(filter);
            return Ok(cars);
        }

        [HttpGet("favorite")]
        [AllowAnonymous]
        public async Task<IActionResult> Favorite(FilterDto filter)
        {
            var userId = User.GetUserId();

            if (!userId.HasValue)
            {
                return BadRequest("User is not authorized");
            }

            var cars = await _carsService.GetUserFavoriteCarsAsync(userId.Value, filter);
            return Ok(cars);
        }

        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> Get(int id)
        {
            var car = await _carsService.GetByIdAsync(id);

            if (car is null)
            {
                return NotFound($"Car with id={id} is not found");
            }

            return Ok(car);
        }

        [HttpGet("{id:int}/editor")]
        [Authorize(Roles = Roles.Admin)] 
        public async Task<IActionResult> GetEditor(int id)
        {
            var car = await _carsService.GetCarEditorDtoByIdAsync(id);
            
            if (car is null)
            {
                return NotFound($"Car with id={id} is not found");
            }

            return Ok(car);
        }

        [HttpPost]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Create([FromBody] CarEditorViewModel car)
        {
            var operation = await _carsService.AddAsync(_mapper.MapToCarEditorDto(car));
            return operation.Success ? 
                CreatedAtAction(nameof(Get), new { id = operation.Result }, car) : 
                BadRequest(operation.FailureMessage);
        }

        [HttpPut("{carId:int}/favorite")]
        [Authorize(Roles = Roles.User)]
        public async Task<IActionResult> ToggleFavorite(int carId)
        {
            var userId = User.GetUserId();

            if (!userId.HasValue)
            {
                return BadRequest("Cannot get user id");
            }

            var operation = await _carsService.ToggleFavoriteAsync(userId.Value, carId);

            return operation.Success ?
                Ok(operation.Result) :
                BadRequest(operation.FailureMessage);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Update(int id, [FromBody] CarEditorViewModel updateCar)
        {
            var carEditor = _mapper.MapToCarEditorDto(updateCar);
            carEditor.Id = id;
            var operation = await _carsService.UpdateAsync(carEditor);
            return operation.Success ?
                NoContent() :
                BadRequest(operation.FailureMessage);
        }

        [HttpPatch("{id:int}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Patch(int id, [FromBody] JsonPatchDocument<CarEditorViewModel> patchCar)
        {
            var car = await _carsService.GetCarEditorDtoByIdAsync(id);
            var carViewModel = _mapper.MapToCarEditorViewModel(car);

            patchCar.ApplyTo(carViewModel, ModelState);
            var updatedCar = _mapper.MapToCarEditorDto(carViewModel);
            updatedCar.Id = car.Id;
            
            var operation = await _carsService.UpdateAsync(updatedCar);
            return operation.Success ?
                NoContent() :
                BadRequest(operation.FailureMessage);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Delete(int id)
        {
            var operation = await _carsService.DeleteByIdAsync(id);
            return operation.Success ?
                NoContent() :
                BadRequest(operation.FailureMessage);
        }
    }
}
