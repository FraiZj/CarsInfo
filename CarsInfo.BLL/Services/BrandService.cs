using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CarsInfo.BLL.Contracts;
using CarsInfo.BLL.Models.Dtos;
using CarsInfo.DAL.Contracts;
using CarsInfo.DAL.Entities;

namespace CarsInfo.BLL.Services
{
    public class BrandService : IBrandService
    {
        private readonly IGenericRepository<Brand> _brandRepository;
        private readonly IMapper _mapper;

        public BrandService(IGenericRepository<Brand> brandRepository, IMapper mapper)
        {
            _brandRepository = brandRepository;
            _mapper = mapper;
        }

        public async Task AddAsync(BrandDto entity)
        {
            var brand = _mapper.Map<Brand>(entity);
            await _brandRepository.AddAsync(brand);
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
