using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CarsInfo.Application.BusinessLogic.Contracts;
using CarsInfo.Application.BusinessLogic.Dtos;
using CarsInfo.Application.BusinessLogic.Validators;
using CarsInfo.Application.Persistence.Contracts;
using CarsInfo.Application.Persistence.Filters;
using CarsInfo.Domain.Entities;
using CarsInfo.Infrastructure.BusinessLogic.Mappers;
using Microsoft.Extensions.Logging;

namespace CarsInfo.Infrastructure.BusinessLogic.Services
{
    public class BrandService : IBrandService
    {
        private readonly IGenericRepository<Brand> _brandRepository;
        private readonly ILogger<BrandService> _logger;
        private readonly BrandServiceMapper _mapper;

        public BrandService(IGenericRepository<Brand> brandRepository, BrandServiceMapper mapper, ILogger<BrandService> logger)
        {
            _brandRepository = brandRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task AddAsync(BrandDto entity)
        {
            try
            {
                ValidationHelper.ThrowIfNull(entity);
                ValidationHelper.ThrowIfStringNullOrWhiteSpace(entity.Name);

                var brand = _mapper.MapToBrand(entity);
                await _brandRepository.AddAsync(brand);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during creating brand");
            }
        }

        public async Task DeleteByIdAsync(int id)
        {
            await _brandRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<BrandDto>> GetAllAsync(string name)
        {
            var filter = new FilterModel();
            if (!string.IsNullOrWhiteSpace(name))
            {
                filter.Filters.Add(new FiltrationField("Name", $"{name}%", "LIKE"));
            }

            var brands = await _brandRepository.GetAllAsync(filter);
            var brandsDtos = _mapper.MapToBrandsDtos(brands);
            return brandsDtos;
        }

        public async Task<BrandDto> GetByIdAsync(int id)
        {
            var brand = await _brandRepository.GetAsync(id);
            var brandDto = _mapper.MapToBrandDto(brand);
            return brandDto;
        }

        public async Task UpdateAsync(BrandDto entity)
        {
            ValidationHelper.ThrowIfNull(entity);
            ValidationHelper.ThrowIfStringNullOrWhiteSpace(entity.Name);

            var brand = _mapper.MapToBrand(entity);
            await _brandRepository.UpdateAsync(brand);
        }
    }
}
