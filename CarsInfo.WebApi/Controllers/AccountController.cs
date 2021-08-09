using System.Threading.Tasks;
using CarsInfo.BLL.Contracts;
using CarsInfo.WebApi.Authorization;
using CarsInfo.WebApi.Mappers;
using CarsInfo.WebApi.ViewModels.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CarsInfo.WebApi.Controllers
{
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenFactory _tokenFactory;
        private readonly AccountControllerMapper _mapper;

        public AccountController(
            IUserService userService,
            AccountControllerMapper mapper,
            ITokenFactory tokenFactory)
        {
            _userService = userService;
            _mapper = mapper;
            _tokenFactory = tokenFactory;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            var user = _mapper.MapToUserDto(model);
            await _userService.AddAsync(user);
            var claims = await _userService.AuthorizeAsync(user);
            var token = _tokenFactory.CreateToken(claims);

            return Ok(token);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            var user = _mapper.MapToUserDto(model);
            var claims = await _userService.AuthorizeAsync(user);
            var token = _tokenFactory.CreateToken(claims);

            return Ok(token);
        }
    }
}
