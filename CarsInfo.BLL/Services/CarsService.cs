using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CarsInfo.BLL.Contracts;
using CarsInfo.BLL.Models.Dtos;
using CarsInfo.DAL.Contracts;
using CarsInfo.DAL.Entities;
using Microsoft.Extensions.Logging;

namespace CarsInfo.BLL.Services
{
    public class CarsService : ICarsService
    {
        private readonly IGenericRepository<Car> _carsRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CarsService> _logger;

        public CarsService(IGenericRepository<Car> carsRepository, IMapper mapper, ILogger<CarsService> logger)
        {
            _carsRepository = carsRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task AddAsync(CarEditorDto entity)
        {
            var car = new Car
            {
                Id = entity.Id,
                BrandId = entity.BrandId,
                Model = entity.Model
            };

            await _carsRepository.AddAsync(car);
        }

        public async Task DeleteByIdAsync(int id)
        {
            await _carsRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<CarDto>> GetAllAsync()
        {
            try
            {
                var cars = await _carsRepository.GetAllAsync();
                var carsDtos = _mapper.Map<IEnumerable<CarDto>>(cars);

                return carsDtos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all cars");
                return new List<CarDto>();
            }
        }

        public async Task<CarDto> GetByIdAsync(int id)
        {
            var car = await _carsRepository.GetAsync(id);
            var carDto = _mapper.Map<CarDto>(car);

            return carDto;
        }

        public async Task UpdateAsync(CarEditorDto entity)
        {
            var car = new Car
            {
                Id = entity.Id,
                BrandId = entity.BrandId,
                Model = entity.Model
            };

            await _carsRepository.UpdateAsync(car);
        }
    }
}
