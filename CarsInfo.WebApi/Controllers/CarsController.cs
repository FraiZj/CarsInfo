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
            var operation = await _carsService.GetAllAsync(filter);
            return operation.Success ?
                Ok(operation.Result) :
                BadRequest(operation.FailureMessage);
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

            var operation = await _carsService.GetUserFavoriteCarsAsync(userId.Value, filter);
            return operation.Success ?
                Ok(operation.Result) :
                BadRequest(operation.FailureMessage);
        }

        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> Get(int id)
        {
            var operation = await _carsService.GetByIdAsync(id);

            if (!operation.Success)
            {
                return BadRequest(operation.FailureMessage);
            }

            return operation.Result is null ? 
                Ok(operation.Result) : 
                NotFound();
        }

        [HttpGet("{id:int}/editor")]
        [Authorize(Roles = Roles.Admin)] 
        public async Task<IActionResult> GetEditor(int id)
        {
            var operation = await _carsService.GetCarEditorDtoByIdAsync(id);

            if (!operation.Success)
            {
                return BadRequest(operation.FailureMessage);
            }

            return operation.Result is null ?
                Ok(operation.Result) :
                NotFound();
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
            var getCarEditorOperation = await _carsService.GetCarEditorDtoByIdAsync(id);

            if (!getCarEditorOperation.Success)
            {
                return BadRequest(getCarEditorOperation.FailureMessage);
            }

            if (getCarEditorOperation.Result is null)
            {
                return BadRequest($"Car with id={id} does not exist");
            }

            var car = getCarEditorOperation.Result;
            var carViewModel = _mapper.MapToCarEditorViewModel(car);
            patchCar.ApplyTo(carViewModel, ModelState);
            var updatedCar = _mapper.MapToCarEditorDto(carViewModel);
            updatedCar.Id = car.Id;
            var updateCarOperation = await _carsService.UpdateAsync(updatedCar);

            return updateCarOperation.Success ?
                NoContent() :
                BadRequest(updateCarOperation.FailureMessage);
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
