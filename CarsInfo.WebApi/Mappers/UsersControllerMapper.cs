using System.Collections.Generic;
using System.Linq;
using CarsInfo.Application.BusinessLogic.Dtos;
using CarsInfo.WebApi.ViewModels.Users;

namespace CarsInfo.WebApi.Mappers
{
    public class UsersControllerMapper
    {
        public UserReadViewModel MapToUserReadViewModel(UserDto userDto)
        {
            if (userDto is null)
            {
                return null;
            }

            return new UserReadViewModel
            {
                Id = userDto.Id,
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                Email = userDto.Email,
                EmailVerified = userDto.EmailVerified,
                Roles = userDto.Roles
            };
        }

        public IEnumerable<UserReadViewModel> MapToUserReadViewModel(IEnumerable<UserDto> userDtos)
        {
            return userDtos.Select(MapToUserReadViewModel);
        }
        
        public UserEditorDto MapToUserEditorDto(UserEditorViewModel model)
        {
            if (model is null)
            {
                return null;
            }

            return new UserEditorDto
            {
                Roles = model.Roles
            };
        }
    }
}