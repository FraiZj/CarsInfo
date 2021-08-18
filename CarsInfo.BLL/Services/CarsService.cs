using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarsInfo.BLL.Assistance;
using CarsInfo.BLL.Contracts;
using CarsInfo.BLL.Mappers;
using CarsInfo.BLL.Models.Dtos;
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
        private readonly IFilterService _filterService;

        public CarsService(
            ICarsRepository carsRepository,
            IGenericRepository<CarPicture> carsPictureRepository,
            ILogger<CarsService> logger,
            CarServiceMapper mapper,
            IFilterService filterService)
        {
            _carsRepository = carsRepository;
            _carsPictureRepository = carsPictureRepository;
            _logger = logger;
            _mapper = mapper;
            _filterService = filterService;
        }

        public async Task AddAsync(CarEditorDto entity)
        {
            try
            {
                ValidateCarEditorDto(entity);
                var car = _mapper.MapToCar(entity);
                var carId = await _carsRepository.AddAsync(car);

                await _carsPictureRepository.AddRangeAsync(entity.CarPicturesUrls.Select(
                    carPicture => new CarPicture
                    {
                        CarId = carId,
                        PictureLink = carPicture
                    }).ToList());
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

        public async Task<IEnumerable<CarDto>> GetAllAsync()
        {
            try
            {
                var cars = await _carsRepository.GetAllWithBrandAndPicturesAsync();
                var carsDtos = _mapper.MapToCarsDtos(cars);
                return carsDtos;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred while fetching all cars");
                return new List<CarDto>();
            }
        }

        public async Task<IEnumerable<CarDto>> GetAllAsync(FilterDto filter)
        {
            try
            {
                if (filter is null)
                {
                    return await GetAllAsync();
                }

                var filters = _filterService.ConfigureCarFilter(filter);
                var cars = await _carsRepository.GetAllWithBrandAndPicturesAsync(filters, filter.Skip, filter.Take);
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
                var oldCar = await _carsRepository.GetWithAllIncludesAsync(entity.Id);

                await _carsRepository.UpdateAsync(car);
                await _carsPictureRepository.DeleteRangeAsync(oldCar.CarPictures.Select(cp => cp.Id));
                await _carsPictureRepository.AddRangeAsync(entity.CarPicturesUrls.Select(
                    carPicture => new CarPicture
                    {
                        CarId = car.Id,
                        PictureLink = carPicture
                    }).ToList());
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
