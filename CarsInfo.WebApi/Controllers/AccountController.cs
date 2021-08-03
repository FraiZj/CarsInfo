using System.Threading.Tasks;
using AutoMapper;
using CarsInfo.BLL.Contracts;
using CarsInfo.BLL.Models.Dtos;
using CarsInfo.WebApi.Authorization;
using CarsInfo.WebApi.ViewModels.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CarsInfo.WebApi.Controllers
{
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly ITokenFactory _tokenFactory;

        public AccountController(IUserService userService, IMapper mapper, ITokenFactory tokenFactory)
        {
            _userService = userService;
            _mapper = mapper;
            _tokenFactory = tokenFactory;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            var user = _mapper.Map<UserDto>(model);
            await _userService.AddAsync(user);
            var claims = await _userService.AuthorizeAsync(user);
            var token = _tokenFactory.CreateToken(claims);

            return Ok(token);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var user = _mapper.Map<UserDto>(model);
            var claims = await _userService.AuthorizeAsync(user);
            var token = _tokenFactory.CreateToken(claims);

            return Ok(token);
        }
    }
}
