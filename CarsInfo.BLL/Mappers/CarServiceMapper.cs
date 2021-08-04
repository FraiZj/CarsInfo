using System.Collections.Generic;
using System.Linq;
using CarsInfo.BLL.Models.Dtos;
using CarsInfo.DAL.Entities;

namespace CarsInfo.BLL.Mappers
{
    public class CarServiceMapper
    {
        private readonly CommentServiceMapper _commentServiceMapper;

        public CarServiceMapper(CommentServiceMapper commentServiceMapper)
        {
            _commentServiceMapper = commentServiceMapper;
        }

        public CarDto MapToCarDto(Car car)
        {
            if (car is null)
            {
                return null;
            }

            return new CarDto
            {
                Id = car.Id,
                Brand = car.Brand?.Name,
                BodyType = car.BodyType?.Name,
                FuelType = car.FuelType?.Name,
                Gearbox = car.Gearbox?.Name,
                Country = car.Country?.Name,
                CarPicturesUrls = car.CarPictures?.Select(cp => cp?.PictureLink).ToList(),
                Description = car.Description,
                Model = car.Model,
                Comments = _commentServiceMapper.MapToCommentsDtos(car.Comments)
            };
        }

        public ICollection<CarDto> MapToCarsDtos(IEnumerable<Car> cars)
        {
            return cars?.Select(MapToCarDto).ToList();
        }

        public CarEditorDto MapToCarEditorDto(Car car)
        {
            if (car is null)
            {
                return null;
            }

            return new CarEditorDto
            {
                Id = car.Id,
                BrandId = car.BrandId,
                BodyTypeId = car.BodyTypeId,
                FuelTypeId = car.FuelTypeId,
                GearboxId = car.GearboxId,
                CountryId = car.CountryId,
                CarPicturesUrls = car.CarPictures?.Select(cp => cp.PictureLink).ToList(),
                Description = car.Description,
                Model = car.Model
            };
        }

        public Car MapToCar(CarEditorDto car)
        {
            if (car is null)
            {
                return null;
            }

            return new Car
            {
                Id = car.Id,
                BrandId = car.BrandId,
                BodyTypeId = car.BodyTypeId,
                FuelTypeId = car.FuelTypeId,
                GearboxId = car.GearboxId,
                CountryId = car.CountryId,
                CarPictures = car.CarPicturesUrls?.Select(cp => new CarPicture { PictureLink = cp}).ToList(),
                Description = car.Description,
                Model = car.Model
            };
        }
    }
}
