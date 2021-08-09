using System.Collections.Generic;
using System.Linq;
using CarsInfo.BLL.Models.Dtos;
using CarsInfo.DAL.Entities;

namespace CarsInfo.BLL.Mappers
{
    public class UserServiceMapper
    {
        private readonly CarServiceMapper _carServiceMapper;

        public UserServiceMapper(CarServiceMapper carServiceMapper)
        {
            _carServiceMapper = carServiceMapper;
        }

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
                FavoriteCars = _carServiceMapper.MapToCarsDtos(user.Cars),
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
            };
        }
    }
}
