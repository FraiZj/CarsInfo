using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarsInfo.Application.BusinessLogic.Contracts;
using CarsInfo.Application.BusinessLogic.Dtos;
using CarsInfo.Application.BusinessLogic.Enums;
using CarsInfo.Application.BusinessLogic.Exceptions;
using CarsInfo.Application.BusinessLogic.Validators;
using CarsInfo.Application.Persistence.Contracts;
using CarsInfo.Application.Persistence.Filters;
using CarsInfo.Domain.Entities;
using CarsInfo.Infrastructure.BusinessLogic.Mappers;
using Microsoft.Extensions.Logging;

namespace CarsInfo.Infrastructure.BusinessLogic.Services
{
    public class CarsService : ICarsService
    {
        private readonly ICarsRepository _carsRepository;
        private readonly IGenericRepository<CarPicture> _carsPictureRepository;
        private readonly IGenericRepository<UserCar> _userCarRepository;
        private readonly ILogger<CarsService> _logger;
        private readonly CarServiceMapper _mapper;
        private readonly IFilterService _filterService;

        public CarsService(
            ICarsRepository carsRepository,
            IGenericRepository<CarPicture> carsPictureRepository,
            IGenericRepository<UserCar> userCarRepository,
            ILogger<CarsService> logger,
            CarServiceMapper mapper,
            IFilterService filterService)
        {
            _carsRepository = carsRepository;
            _carsPictureRepository = carsPictureRepository;
            _userCarRepository = userCarRepository;
            _logger = logger;
            _mapper = mapper;
            _filterService = filterService;
        }

        public async Task<bool> AddAsync(CarEditorDto entity)
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

                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred while creating car");
                return false;
            }
        }

        public async Task<ToggleFavoriteStatus> ToggleFavoriteAsync(int userId, int carId)
        {
            try
            {
                var car = await _carsRepository.GetAsync(carId);
                ValidationHelper.ThrowIfNull(car);

                var userCar = await _userCarRepository.GetAsync(new List<FiltrationField>
                {
                    new("CarId", car.Id, separator: "AND"),
                    new("UserId", userId)
                }, true);

                if (userCar is null || userCar.IsDeleted)
                {
                    return await AddToFavoriteAsync(userCar, carId, userId);
                }

                return await DeleteFromFavoriteAsync(userCar.Id);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred while adding car with to favorite");
                return ToggleFavoriteStatus.Error;
            }
        }

        private async Task<ToggleFavoriteStatus> AddToFavoriteAsync(UserCar userCar, int carId, int userId)
        {
            if (userCar is null)
            {
                await _userCarRepository.AddAsync(new UserCar
                {
                    CarId = carId,
                    UserId = userId
                });
                return ToggleFavoriteStatus.AddedToFavorite;
            }

            userCar.IsDeleted = false;
            await _userCarRepository.UpdateAsync(userCar);
            return ToggleFavoriteStatus.AddedToFavorite;
        }

        private async Task<ToggleFavoriteStatus> DeleteFromFavoriteAsync(int userCarId)
        {
            await _userCarRepository.DeleteAsync(userCarId);
            return ToggleFavoriteStatus.DeleteFromFavorite;
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

        public async Task<IEnumerable<CarDto>> GetAllAsync(FilterDto filterDto)
        {
            try
            {
                if (filterDto is null)
                {
                    return await GetAllAsync();
                }

                var filter = _filterService.ConfigureCarFilter(filterDto);
                var cars = await _carsRepository.GetAllWithBrandAndPicturesAsync(filter);
                var currentUserFavoriteCars = await GetUserCarAsync(filterDto.CurrentUserId);
                var carsDtos = _mapper.MapToCarsDtos(cars, currentUserFavoriteCars);

                return carsDtos;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred while fetching all cars");
                return new List<CarDto>();
            }
        }

        public async Task<IEnumerable<CarDto>> GetUserCarsAsync(FilterDto filter)
        {
            try
            {
                ValidationHelper.ThrowIfNull(filter);
                ValidationHelper.ThrowIfStringNullOrWhiteSpace(filter.CurrentUserId);

                var filterModel = _filterService.ConfigureCarFilter(filter);
                filterModel.Filters.Add(new FiltrationField("UserCar.IsDeleted", 0));
                var cars = await _carsRepository.GetUserCarsAsync(filter.CurrentUserId, filterModel);
                var carsDtos = _mapper.MapToCarsDtos(cars).ToList();
                carsDtos.ForEach(c => c.IsLiked = true);

                return carsDtos;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred while fetching all cars");
                return new List<CarDto>();
            }
        }
        
        private async Task<ICollection<UserCar>> GetUserCarAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return null;
            }

            return (await _userCarRepository.GetAllAsync(new List<FiltrationField>
            {
                new("UserId", int.Parse(id))
            })).ToList();
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

            if (!car.CarPicturesUrls?.Any() ?? true)
            {
                throw new BllException("Car should have at least one picture");
            }
        }
    }
}
