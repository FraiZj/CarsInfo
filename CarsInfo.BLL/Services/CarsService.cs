using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CarsInfo.BLL.Assistance;
using CarsInfo.BLL.Contracts;
using CarsInfo.BLL.Models.Dtos;
using CarsInfo.DAL.Contracts;
using CarsInfo.DAL.Entities;
using Microsoft.Extensions.Logging;

namespace CarsInfo.BLL.Services
{
    public class CarsService : ICarsService
    {
        private readonly ICarsRepository _carsRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CarsService> _logger;

        public CarsService(
            ICarsRepository carsRepository,
            IMapper mapper, 
            ILogger<CarsService> logger)
        {
            _carsRepository = carsRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task AddAsync(CarEditorDto entity)
        {
            try
            {
                ValidateCarEditorDto(entity);
                var car = _mapper.Map<Car>(entity);
                await _carsRepository.AddAsync(car);
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
                var carsDtos = _mapper.Map<IEnumerable<CarDto>>(cars);
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
                var carDto = _mapper.Map<CarDto>(car);

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
                var car = _mapper.Map<Car>(entity);
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
