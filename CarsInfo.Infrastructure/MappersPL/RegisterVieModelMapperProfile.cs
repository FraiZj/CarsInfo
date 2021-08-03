using AutoMapper;
using CarsInfo.BLL.Models.Dtos;
using CarsInfo.WebApi.ViewModels.ViewModels;

namespace CarsInfo.Infrastructure.MappersPL
{
    public class RegisterVieModelMapperProfile : Profile
    {
        public RegisterVieModelMapperProfile()
        {
            CreateMap<RegisterViewModel, UserDto>();
        }
    }
}
