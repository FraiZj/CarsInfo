using CarsInfo.BLL.Models.Dtos;
using CarsInfo.WebApi.ViewModels.ViewModels;

namespace CarsInfo.WebApi.Mappers
{
    public class AccountControllerMapper
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
    }
}
