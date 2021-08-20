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
                Model = car.Model,
                Comments = _commentServiceMapper.MapToCommentsDtos(car.Comments)
            };
        }

        public CarDto MapToCarDto(Car car, ICollection<UserCar> userCars)
        {
            if (car is null)
            {
                return null;
            }

            if (!userCars?.Any() ?? true)
            {
                return MapToCarDto(car);
            }

            return new CarDto
            {
                Id = car.Id,
                Brand = car.Brand?.Name,
                CarPicturesUrls = car.CarPictures?.Select(cp => cp?.PictureLink).ToList(),
                Description = car.Description,
                Model = car.Model,
                Comments = _commentServiceMapper.MapToCommentsDtos(car.Comments),
                IsLiked = userCars!.Any(c => c.CarId == car.Id)
            };
        }

        public ICollection<CarDto> MapToCarsDtos(IEnumerable<Car> cars)
        {
            return cars?.Select(MapToCarDto).ToList();
        }

        public ICollection<CarDto> MapToCarsDtos(IEnumerable<Car> cars, ICollection<UserCar> userCars)
        {
            if (!userCars?.Any() ?? true)
            {
                return MapToCarsDtos(cars);
            }

            return cars?.Select(car => MapToCarDto(car, userCars)).ToList();
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
