using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarsInfo.Application.BusinessLogic.Contracts;
using CarsInfo.Application.BusinessLogic.Dtos;
using CarsInfo.Application.BusinessLogic.Enums;
using CarsInfo.Application.BusinessLogic.OperationResult;
using CarsInfo.Application.Persistence.Contracts;
using CarsInfo.Application.Persistence.Filters;
using CarsInfo.Domain.Entities;
using CarsInfo.Infrastructure.BusinessLogic.Mappers;

namespace CarsInfo.Infrastructure.BusinessLogic.Services
{
    public class CarsService : ICarsService
    {
        private readonly ICarsRepository _carsRepository;
        private readonly IGenericRepository<CarPicture> _carsPictureRepository;
        private readonly IUsersRepository _usersRepository;
        private readonly IGenericRepository<UserCar> _userCarRepository;
        private readonly CarServiceMapper _mapper;
        private readonly IFilterService _filterService;

        public CarsService(
            ICarsRepository carsRepository,
            IGenericRepository<CarPicture> carsPictureRepository,
            IUsersRepository usersRepository,
            IGenericRepository<UserCar> userCarRepository,
            CarServiceMapper mapper,
            IFilterService filterService)
        {
            _carsRepository = carsRepository;
            _carsPictureRepository = carsPictureRepository;
            _usersRepository = usersRepository;
            _userCarRepository = userCarRepository;
            _mapper = mapper;
            _filterService = filterService;
        }

        public async Task<OperationResult<int>> AddAsync(CarEditorDto entity)
        {
            if (!entity.CarPicturesUrls?.Any() ?? true)
            {
                return OperationResult<int>.FailureResult("Car should have at least one picture");
            }
                
            var carId = await _carsRepository.AddAsync(_mapper.MapToCar(entity));
            await _carsPictureRepository.AddRangeAsync(entity.CarPicturesUrls.Select(
                carPicture => new CarPicture
                {
                    CarId = carId,
                    PictureLink = carPicture
                }).ToList());

            return OperationResult<int>.SuccessResult(carId);
        }

        public async Task<OperationResult<ToggleFavoriteStatus>> ToggleFavoriteAsync(int userId, int carId)
        {
            var car = await _carsRepository.GetAsync(carId);
            if (car is null)
            {
                return OperationResult<ToggleFavoriteStatus>.FailureResult($"Car with id={carId} does not exist");
            }

            var user = await _usersRepository.GetAsync(userId);
            if (user is null)
            {
                return OperationResult<ToggleFavoriteStatus>.FailureResult($"User with id={carId} does not exist");
            }
                
            var userCar = await _userCarRepository.GetAsync(new List<FiltrationField>
            {
                new("CarId", car.Id, separator: "AND"),
                new("UserId", userId)
            }, true);

            if (userCar is null)
            {
                await _userCarRepository.AddAsync(new UserCar
                {
                    CarId = carId,
                    UserId = userId
                });
                return OperationResult<ToggleFavoriteStatus>.SuccessResult(ToggleFavoriteStatus.AddedToFavorite);
            }

            if (userCar.IsDeleted)
            {
                userCar.IsDeleted = false;
                await _userCarRepository.UpdateAsync(userCar);
                return OperationResult<ToggleFavoriteStatus>.SuccessResult(ToggleFavoriteStatus.AddedToFavorite);
            }

            await _userCarRepository.DeleteAsync(userCar.Id);
            return OperationResult<ToggleFavoriteStatus>.SuccessResult(ToggleFavoriteStatus.DeleteFromFavorite);
        }

        public async Task<OperationResult> DeleteByIdAsync(int id)
        {
            var car = await _carsRepository.GetAsync(id);
            if (car is null)
            {
                return OperationResult.FailureResult($"Car with id={id} does not exist");
            }
                
            await _carsRepository.DeleteAsync(id);
            return OperationResult.SuccessResult();
        }

        public async Task<OperationResult<IEnumerable<CarDto>>> GetAllAsync()
        {
            var cars = await _carsRepository.GetAllAsync();
            return OperationResult<IEnumerable<CarDto>>.SuccessResult(_mapper.MapToCarsDtos(cars));
        }

        public async Task<OperationResult<IEnumerable<CarDto>>> GetAllAsync(CarFilterDto carFilterDto)
        {
            if (carFilterDto is null)
            {
                return await GetAllAsync();
            }

            var filter = _filterService.ConfigureCarFilter(carFilterDto);
            var cars = await _carsRepository.GetAllAsync(filter);
            return OperationResult<IEnumerable<CarDto>>.SuccessResult(_mapper.MapToCarsDtos(cars));
        }

        public async Task<OperationResult<IEnumerable<CarDto>>> GetUserFavoriteCarsAsync(int userId, CarFilterDto carFilter)
        {
            var user = await _usersRepository.GetAsync(userId);
            if (user is null)
            {
                return OperationResult<IEnumerable<CarDto>>.FailureResult($"User with id={userId} does not exist");
            }

            var filterModel = _filterService.ConfigureCarFilter(carFilter);
            filterModel.Filters.Add(new FiltrationField("UserCar.IsDeleted", 0));
            var cars = await _carsRepository.GetUserFavoriteCarsAsync(userId, filterModel);

            return OperationResult<IEnumerable<CarDto>>.SuccessResult(_mapper.MapToCarsDtos(cars));
        }

        public async Task<OperationResult<IEnumerable<int>>> GetUserFavoriteCarsIdsAsync(int userId)
        {
            var user = await _usersRepository.GetAsync(userId);
            if (user is null)
            {
                return OperationResult<IEnumerable<int>>.FailureResult($"User with id={userId} does not exist");
            }

            var filterModel = new FilterModel(new FiltrationField("UserCar.UserId", userId));
            var userCars = await _userCarRepository.GetAllAsync(filterModel);

            return OperationResult<IEnumerable<int>>.SuccessResult(userCars.Select(userCar => userCar.CarId));
        }
        
        public async Task<OperationResult<CarDto>> GetByIdAsync(int id)
        {
            var car = await _carsRepository.GetAsync(id);
            return OperationResult<CarDto>.SuccessResult(_mapper.MapToCarDto(car));
        }

        public async Task<OperationResult<CarEditorDto>> GetCarEditorDtoByIdAsync(int id)
        {
            var car = await _carsRepository.GetAsync(id);
            return OperationResult<CarEditorDto>.SuccessResult(_mapper.MapToCarEditorDto(car));
        }

        public async Task<OperationResult> UpdateAsync(CarEditorDto entity)
        {
            if (!entity.CarPicturesUrls?.Any() ?? true)
            {
                return OperationResult.FailureResult("Car should have at least one picture");
            }
                
            var car = await _carsRepository.GetAsync(entity.Id);
            if (car is null)
            {
                return OperationResult.FailureResult($"Car with id={entity.Id} does not exist");
            }
                
            await _carsRepository.UpdateAsync(_mapper.MapToCar(entity));
            await _carsPictureRepository.DeleteRangeAsync(car.CarPictures.Select(cp => cp.Id));
            await _carsPictureRepository.AddRangeAsync(entity.CarPicturesUrls.Select(
                carPicture => new CarPicture
                {
                    CarId = entity.Id,
                    PictureLink = carPicture
                }).ToList());
            return OperationResult.SuccessResult();
        }
    }
}
