using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CarsInfo.Application.BusinessLogic.Contracts;
using CarsInfo.Application.BusinessLogic.Dtos;
using CarsInfo.Application.BusinessLogic.OperationResult;
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

        public BrandService(
            IGenericRepository<Brand> brandRepository, 
            BrandServiceMapper mapper, 
            ILogger<BrandService> logger)
        {
            _brandRepository = brandRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<OperationResult<int>> AddAsync(BrandDto entity)
        {
            try
            {
                var filter = new FilterModel(new FiltrationField("LOWER(Brand.Name)", entity.Name.ToLower()));
                var brand = await _brandRepository.GetAsync(filter.Filters);
                if (brand is not null)
                {
                    return OperationResult<int>.FailureResult($"Brand with name='{entity.Name}' already exists");
                }
                
                var newBrand = _mapper.MapToBrand(entity);
                var id = await _brandRepository.AddAsync(newBrand);
                return OperationResult<int>.SuccessResult(id);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred during creating brand");
                return OperationResult<int>.ExceptionResult();
            }
        }

        public async Task<OperationResult> DeleteByIdAsync(int id)
        {
            try
            {
                var brand = await _brandRepository.GetAsync(id);
                if (brand is null)
                {
                    return OperationResult.FailureResult($"Brand with id={id} does not exists");
                }
                
                await _brandRepository.DeleteAsync(id);
                return OperationResult.SuccessResult();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred during deleting brand");
                return OperationResult.ExceptionResult();
            }
        }

        public async Task<OperationResult<IEnumerable<BrandDto>>> GetAllAsync(string name)
        {
            try
            {
                var filter = !string.IsNullOrWhiteSpace(name) ? 
                    new FilterModel(new FiltrationField("Brand.Name", $"{name}%", "LIKE")) :
                    new FilterModel();

                var brands = await _brandRepository.GetAllAsync(filter);
                return OperationResult<IEnumerable<BrandDto>>.SuccessResult(_mapper.MapToBrandsDtos(brands));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred during fetching brands");
                return OperationResult<IEnumerable<BrandDto>>.ExceptionResult();
            }
        }

        public async Task<OperationResult<BrandDto>> GetByIdAsync(int id)
        {
            try
            {
                var brand = await _brandRepository.GetAsync(id);
                return brand is null ? 
                    OperationResult<BrandDto>.FailureResult($"Brand with id={id} not found") : 
                    OperationResult<BrandDto>.SuccessResult(_mapper.MapToBrandDto(brand));
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"An error occurred during fetching brand with id={id}");
                return OperationResult<BrandDto>.ExceptionResult();
            }
        }

        public async Task<OperationResult> UpdateAsync(BrandDto entity)
        {
            try
            {
                var brand = await _brandRepository.GetAsync(entity.Id);
                if (brand is null)
                {
                    return OperationResult.FailureResult($"Brand with id={entity.Id} does not exists");
                }
                
                await _brandRepository.UpdateAsync(_mapper.MapToBrand(entity));
                return OperationResult.SuccessResult();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred updating deleting brand");
                return OperationResult.ExceptionResult();
            }
        }
    }
}
