using AutoMapper;
using CarsInfo.BLL.Models.Dtos;
using CarsInfo.DAL.Entities;

namespace CarsInfo.Infrastructure.MappersBLL
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
