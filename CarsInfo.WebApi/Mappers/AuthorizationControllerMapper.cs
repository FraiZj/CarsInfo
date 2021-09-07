using CarsInfo.Application.BusinessLogic.Dtos;
using CarsInfo.WebApi.ViewModels;

namespace CarsInfo.WebApi.Mappers
{
    public class AuthorizationControllerMapper
    {
        public UserDto MapToUserDto(LoginViewModel model)
        {
            if (model is null)
            {
                return null;
            }

            return new UserDto
            {
                Email = model.Email,
                Password = model.Password
            };
        }

        public UserDto MapToUserDto(RegisterViewModel model)
        {
            if (model is null)
            {
                return null;
            }

            return new UserDto
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Password = model.Password
            };
        }

        public UserViewModel MapToUserViewModel(UserDto user)
        {
            if (user is null)
            {
                return null;
            }

            return new UserViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Password = user.Password,
                Roles = user.Roles
            };
        }
    }
}
