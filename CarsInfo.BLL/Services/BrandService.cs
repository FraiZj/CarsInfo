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
            var filters = new List<FilterModel>();
            if (!string.IsNullOrWhiteSpace(name))
            {
                filters = new List<FilterModel>
                {
                    new("Name", $"{name}%", "LIKE")
                };
            }

            var brands = await _brandRepository.GetAllAsync(filters);
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
