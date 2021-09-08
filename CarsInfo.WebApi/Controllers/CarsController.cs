using System.Linq;
using System.Threading.Tasks;
using CarsInfo.Application.BusinessLogic.Contracts;
using CarsInfo.Application.BusinessLogic.Dtos;
using CarsInfo.Application.BusinessLogic.Enums;
using CarsInfo.WebApi.Controllers.Base;
using CarsInfo.WebApi.Extensions;
using CarsInfo.WebApi.Mappers;
using CarsInfo.WebApi.ViewModels.Car;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CarsInfo.WebApi.Controllers
{
    [Route("cars")]
    public class CarsController : AppControllerBase
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
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var carEditor = _mapper.MapToCarEditorDto(car);
            var created = await _carsService.AddAsync(carEditor);

            return created ? 
                Created("", car) : 
                BadRequest("Car was not created.");
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

            var status = await _carsService.ToggleFavoriteAsync(userId.Value, carId);

            if (status == ToggleFavoriteStatus.Error)
            {
                return BadRequest("Cannot toggle favorite car with for user");
            }

            return Ok(status);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Update(int id, [FromBody] CarEditorViewModel updateCar)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var carEditor = _mapper.MapToCarEditorDto(updateCar);
            carEditor.Id = id;
            await _carsService.UpdateAsync(carEditor);
            return Ok(carEditor);
        }

        [HttpPatch("{id:int}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Patch(int id, [FromBody] JsonPatchDocument<CarEditorViewModel> patchCar)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var car = await _carsService.GetCarEditorDtoByIdAsync(id);
            var carViewModel = _mapper.MapToCarEditorViewModel(car);

            patchCar.ApplyTo(carViewModel, ModelState);
            var updatedCar = _mapper.MapToCarEditorDto(carViewModel);
            updatedCar.Id = car.Id;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values.First().Errors.First().ErrorMessage);
            }

            await _carsService.UpdateAsync(updatedCar);
            return Ok(car);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Delete(int id)
        {
            await _carsService.DeleteByIdAsync(id);
            return Ok($"Car with id={id} has been deleted");
        }
    }
}
