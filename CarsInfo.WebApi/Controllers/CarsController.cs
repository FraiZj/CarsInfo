﻿using System.Collections.Generic;
using System.Threading.Tasks;
using CarsInfo.BLL.Contracts;
using CarsInfo.BLL.Models.Dtos;
using CarsInfo.BLL.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CarsInfo.WebApi.Controllers
{
    [Route("cars")]
    [Authorize(Roles = Roles.Admin)]
    public class CarsController : ControllerBase
    {
        private readonly ICarsService _carsService;

        public CarsController(ICarsService carsService)
        {
            _carsService = carsService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IEnumerable<CarDto>> Get(IEnumerable<string> brands = null)
        {
            var cars = await _carsService.GetAllAsync(brands);
            return cars;
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

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CarEditorDto car)
        {
            await _carsService.AddAsync(car);
            return Created("", car);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] CarEditorDto car)
        {
            await _carsService.UpdateAsync(car);
            return Ok(car);
        }

        [HttpPatch("{id:int}")]
        public async Task<IActionResult> Patch(int id, [FromBody] JsonPatchDocument<CarEditorDto> patchCar)
        {
            var car = await _carsService.GetCarEditorDtoByIdAsync(id);
            patchCar.ApplyTo(car, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest("Model state is invalid.");
            }

            await _carsService.UpdateAsync(car);
            return Ok(car);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _carsService.DeleteByIdAsync(id);
            return Ok($"Car with id={id} has been deleted");
        }
    }
}
