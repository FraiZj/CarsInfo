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
    public class BrandService : IBrandService
    {
        private readonly IGenericRepository<Brand> _brandRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<BrandService> _logger;

        public BrandService(IGenericRepository<Brand> brandRepository, IMapper mapper, ILogger<BrandService> logger)
        {
            _brandRepository = brandRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task AddAsync(BrandDto entity)
        {
            try
            {
                var brand = _mapper.Map<Brand>(entity);
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
            var brandsDtos = _mapper.Map<IEnumerable<BrandDto>>(brands);
            return brandsDtos;
        }

        public async Task<BrandDto> GetByIdAsync(int id)
        {
            var brand = await _brandRepository.GetAsync(id);
            var brandDto = _mapper.Map<BrandDto>(brand);
            return brandDto;
        }

        public async Task UpdateAsync(BrandDto entity)
        {
            var brand = _mapper.Map<Brand>(entity);
            await _brandRepository.UpdateAsync(brand);
        }
    }
}
