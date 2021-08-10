using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CarsInfo.BLL.Assistance;
using CarsInfo.BLL.Contracts;
using CarsInfo.BLL.Mappers;
using CarsInfo.BLL.Models.Dtos;
using CarsInfo.DAL.Assistance;
using CarsInfo.DAL.Contracts;
using CarsInfo.DAL.Entities;
using Microsoft.Extensions.Logging;

namespace CarsInfo.BLL.Services
{
    public class CarsService : ICarsService
    {
        private readonly ICarsRepository _carsRepository;
        private readonly IGenericRepository<CarPicture> _carsPictureRepository;
        private readonly ILogger<CarsService> _logger;
        private readonly CarServiceMapper _mapper;

        public CarsService(
            ICarsRepository carsRepository,
            IGenericRepository<CarPicture> carsPictureRepository,
            ILogger<CarsService> logger,
            CarServiceMapper mapper)
        {
            _carsRepository = carsRepository;
            _carsPictureRepository = carsPictureRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task AddAsync(CarEditorDto entity)
        {
            try
            {
                ValidateCarEditorDto(entity);
                var car = _mapper.MapToCar(entity);
                var carId = await _carsRepository.AddAsync(car);

                foreach (var carPicture in entity.CarPicturesUrls)
                {
                    await _carsPictureRepository.AddAsync(new CarPicture
                    {
                        CarId = carId,
                        PictureLink = carPicture
                    });
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred while creating car");
            }
        }

        public async Task DeleteByIdAsync(int id)
        {
            try
            {
                await _carsRepository.DeleteAsync(id);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"An error occurred while deleting car with id={id}");
            }
        }

        public async Task<IEnumerable<CarDto>> GetAllAsync(string brand = null)
        {
            try
            {
                var cars = string.IsNullOrWhiteSpace(brand) ?
                    await _carsRepository.GetAllWithBrandAndPicturesAsync() :
                    await _carsRepository.GetAllWithBrandAndPicturesAsync(new List<FilterModel>
                    {
                        new("Brand.Name", $"{brand}%", "LIKE")
                    });
                var carsDtos = _mapper.MapToCarsDtos(cars);
                return carsDtos;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred while fetching all cars");
                return new List<CarDto>();
            }
        }

        public async Task<CarDto> GetByIdAsync(int id)
        {
            try
            {
                var car = await _carsRepository.GetWithAllIncludesAsync(id);
                var carDto = _mapper.MapToCarDto(car);

                return carDto;
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"An error occurred while fetching car with id={id}");
                return null;
            }
        }

        public async Task<CarEditorDto> GetCarEditorDtoByIdAsync(int id)
        {
            try
            {
                var car = await _carsRepository.GetWithAllIncludesAsync(id);
                var carDto = _mapper.MapToCarEditorDto(car);

                return carDto;
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"An error occurred while fetching car with id={id}");
                return null;
            }
        }

        public async Task UpdateAsync(CarEditorDto entity)
        {
            try
            {
                ValidateCarEditorDto(entity);
                var car = _mapper.MapToCar(entity);
                await _carsRepository.UpdateAsync(car);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"An error occurred while updating car with id={entity.Id}");
            }
        }

        private static void ValidateCarEditorDto(CarEditorDto car)
        {
            ValidationHelper.ThrowIfNull(car);
            ValidationHelper.ThrowIfStringNullOrWhiteSpace(car.Model);
        }
    }
}
