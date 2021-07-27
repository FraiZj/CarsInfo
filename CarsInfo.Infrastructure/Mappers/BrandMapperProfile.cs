using AutoMapper;
using CarsInfo.BLL.Models.Dtos;
using CarsInfo.DAL.Entities;

namespace CarsInfo.Infrastructure.Mappers
{
    public class BrandMapperProfile : Profile
    {
        public BrandMapperProfile()
        {
            CreateMap<Brand, BrandDto>()
                .ReverseMap();
        }
    }
}
