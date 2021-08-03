using System.Linq;
using AutoMapper;
using CarsInfo.BLL.Models.Dtos;
using CarsInfo.DAL.Entities;

namespace CarsInfo.Infrastructure.Mappers
{
    public class CarMapperProfile : Profile
    {
        public CarMapperProfile()
        {
            CreateMap<Car, CarDto>()
                .ForMember(carDto => carDto.Brand, opt => opt.MapFrom(car => car.Brand.Name))
                .ForMember(carDto => carDto.FuelType, opt => opt.MapFrom(car => car.FuelType.Name))
                .ForMember(carDto => carDto.BodyType, opt => opt.MapFrom(car => car.BodyType.Name))
                .ForMember(carDto => carDto.Country, opt => opt.MapFrom(car => car.Country.Name))
                .ForMember(carDto => carDto.Gearbox, opt => opt.MapFrom(car => car.Gearbox.Name))
                .ForMember(carDto => carDto.CarPicturesUrls, opt => opt.MapFrom(car => car.CarPictures.Select(cp => cp.PictureLink)));

            CreateMap<CarEditorDto, Car>()
                .ForMember(carDto => carDto.BrandId, opt => opt.MapFrom(car => car.BrandId))
                .ForMember(carDto => carDto.FuelTypeId, opt => opt.MapFrom(car => car.FuelTypeId))
                .ForMember(carDto => carDto.BodyTypeId, opt => opt.MapFrom(car => car.BodyTypeId))
                .ForMember(carDto => carDto.CountryId, opt => opt.MapFrom(car => car.CountryId))
                .ForMember(carDto => carDto.GearboxId, opt => opt.MapFrom(car => car.GearboxId))
                .ForMember(carDto => carDto.CarPictures, opt => opt.MapFrom(car => car.CarPicturesUrls.Select(cp => new CarPicture { PictureLink = cp })));
        }
    }
}
