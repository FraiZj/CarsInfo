using System.Collections.Generic;
using System.Linq;
using CarsInfo.Application.BusinessLogic.Dtos;
using CarsInfo.Domain.Entities;

namespace CarsInfo.Infrastructure.BusinessLogic.Mappers
{
    public class UserServiceMapper
    {
        public UserDto MapToUserDto(User user)
        {
            if (user is null)
            {
                return null;
            }

            return new UserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                IsExternal = user.IsExternal,
                Roles = user.Roles.Select(r => r.Name).ToList()
            };
        }

        public ICollection<UserDto> MapToUsersDtos(IEnumerable<User> users)
        {
            return users?.Select(MapToUserDto).ToList();
        }

        public User MapToUser(UserDto user)
        {
            if (user is null)
            {
                return null;
            }

            return new User
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                IsExternal = user.IsExternal,
                Password = user.Password
            };
        }
    }
}
