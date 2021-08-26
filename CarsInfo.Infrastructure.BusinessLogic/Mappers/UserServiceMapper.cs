using System.Collections.Generic;
using System.Linq;
using CarsInfo.Application.BusinessLogic.Dtos;
using CarsInfo.Domain.Entities;

namespace CarsInfo.Infrastructure.BusinessLogic.Mappers
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
                RefreshToken = user.RefreshToken,
                RefreshTokenExpiryTime = user.RefreshTokenExpiryTime,
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
                Password = user.Password,
                RefreshToken = user.RefreshToken,
                RefreshTokenExpiryTime = user.RefreshTokenExpiryTime
            };
        }
    }
}
