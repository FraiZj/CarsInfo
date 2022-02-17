using System.Collections.Generic;
using System.Threading.Tasks;
using CarsInfo.Application.BusinessLogic.Contracts;
using CarsInfo.Application.BusinessLogic.Enums;
using CarsInfo.WebApi.Controllers.Base;
using CarsInfo.WebApi.Mappers;
using CarsInfo.WebApi.ViewModels.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarsInfo.WebApi.Controllers
{
    [ApiController, Route("users"), Authorize(Roles = Roles.Admin)]
    public class UsersController : AppController
    {
        private readonly IUserService _userService;
        private readonly UsersControllerMapper _mapper;

        public UsersController(
            IUserService userService,
            UsersControllerMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserReadViewModel>>> GetAllAsync()
        {
            var getAllUsersOperation = await _userService.GetAllAsync();

            return getAllUsersOperation.Success
                ? Ok(_mapper.MapToUserReadViewModel(getAllUsersOperation.Result))
                : BadRequest(getAllUsersOperation.FailureMessage);
        }
        
        [HttpGet("{email}")]
        public async Task<ActionResult<UserReadViewModel>> GetByEmailAsync(
            [FromRoute] string email)
        {
            var getUserOperation = await _userService.GetByEmailAsync(email);

            if (!getUserOperation.Success)
            {
                return BadRequest(getUserOperation.FailureMessage);
            }
            
            return getUserOperation.Result is null
                ? NotFound()
                : Ok(_mapper.MapToUserReadViewModel(getUserOperation.Result));
        }
        
        [HttpPut("{email}")]
        public async Task<IActionResult> UpdateAsync(
            [FromRoute] string email, 
            [FromBody] UserEditorViewModel model)
        {
            var updateUserOperation = await _userService.UpdateAsync(email ,_mapper.MapToUserEditorDto(model));
            return updateUserOperation.Success
                ? NoContent()
                : BadRequest(updateUserOperation.FailureMessage);
        }
        
        [HttpDelete("{email}")]
        public async Task<IActionResult> DeleteAsync(
            [FromRoute] string email)
        {
            var updateUserOperation = await _userService.DeleteByByEmailAsync(email);
            return updateUserOperation.Success
                ? NoContent()
                : BadRequest(updateUserOperation.FailureMessage);
        }
    }
}