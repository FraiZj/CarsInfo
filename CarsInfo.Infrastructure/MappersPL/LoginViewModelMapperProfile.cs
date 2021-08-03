using AutoMapper;
using CarsInfo.DAL.Entities;
using CarsInfo.WebApi.ViewModels.ViewModels;

namespace CarsInfo.Infrastructure.MappersPL
{
    public class LoginViewModelMapperProfile : Profile
    {
        public LoginViewModelMapperProfile()
        {
            CreateMap<User, LoginViewModel>()
                .ReverseMap();
        }
    }
}
