using AutoMapper;
using CarsInfo.BLL.Models.Dtos;
using CarsInfo.DAL.Entities;

namespace CarsInfo.Infrastructure.Mappers
{
    public class UserMapperProfile : Profile
    {
        public UserMapperProfile()
        {
            CreateMap<User, UserDto>()
                .ForMember(userDto => userDto.Password, opt => opt.Ignore());

            CreateMap<UserDto, User>()
                .ForMember(user => user.Password, opt => opt.MapFrom(
                    userDto => BCrypt.Net.BCrypt.HashPassword(userDto.Password)));
        }
    }
}
