using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CarsInfo.BLL.Contracts;
using CarsInfo.BLL.Mappers;
using CarsInfo.BLL.Models.Dtos;
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

        public async Task<IEnumerable<BrandDto>> GetAllAsync()
        {
            var brands = await _brandRepository.GetAllAsync();
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
            var brand = _mapper.MapToBrand(entity);
            await _brandRepository.UpdateAsync(brand);
        }
    }
}
