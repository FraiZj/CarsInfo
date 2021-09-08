using System.Collections.Generic;
using System.Linq;
using CarsInfo.Application.BusinessLogic.Dtos;
using CarsInfo.Domain.Entities;

namespace CarsInfo.Infrastructure.BusinessLogic.Mappers
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
                CarPicturesUrls = car.CarPictures?.Select(cp => cp?.PictureLink).ToList(),
                Description = car.Description,
                Model = car.Model
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
                CarPictures = car.CarPicturesUrls?.Select(cp => new CarPicture { PictureLink = cp}).ToList(),
                Description = car.Description,
                Model = car.Model
            };
        }
    }
}
