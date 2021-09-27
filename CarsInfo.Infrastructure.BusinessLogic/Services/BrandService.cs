using System.Collections.Generic;
using System.Threading.Tasks;
using CarsInfo.Application.BusinessLogic.Contracts;
using CarsInfo.Application.BusinessLogic.Dtos;
using CarsInfo.Application.BusinessLogic.OperationResult;
using CarsInfo.Application.Persistence.Contracts;
using CarsInfo.Application.Persistence.Filters;
using CarsInfo.Domain.Entities;
using CarsInfo.Infrastructure.BusinessLogic.Mappers;

namespace CarsInfo.Infrastructure.BusinessLogic.Services
{
    public class BrandService : IBrandService
    {
        private readonly IGenericRepository<Brand> _brandRepository;
        private readonly BrandServiceMapper _mapper;

        public BrandService(
            IGenericRepository<Brand> brandRepository, 
            BrandServiceMapper mapper)
        {
            _brandRepository = brandRepository;
            _mapper = mapper;
        }

        public async Task<OperationResult<int>> AddAsync(BrandDto entity)
        {
            var filter = new FilterModel(new FiltrationField("LOWER(Brand.Name)", entity.Name.ToLower()));
            if (await _brandRepository.ContainsAsync(filter.Filters))
            {
                return OperationResult<int>.FailureResult($"Brand with name='{entity.Name}' already exists");
            }
                
            var newBrand = _mapper.MapToBrand(entity);
            var id = await _brandRepository.AddAsync(newBrand);
            return OperationResult<int>.SuccessResult(id);
        }

        public async Task<OperationResult> DeleteByIdAsync(int id)
        {
            var brand = await _brandRepository.GetAsync(id);
            if (brand is null)
            {
                return OperationResult.FailureResult($"Brand with id={id} does not exists");
            }
                
            await _brandRepository.DeleteAsync(id);
            return OperationResult.SuccessResult();
        }

        public async Task<OperationResult<IEnumerable<BrandDto>>> GetAllAsync(string name)
        {
            var filter = !string.IsNullOrWhiteSpace(name) ? 
                new FilterModel(new FiltrationField("Brand.Name", $"{name}%", "LIKE")) :
                new FilterModel();

            var brands = await _brandRepository.GetAllAsync(filter);
            return OperationResult<IEnumerable<BrandDto>>.SuccessResult(_mapper.MapToBrandsDtos(brands));
        }

        public async Task<OperationResult<BrandDto>> GetByIdAsync(int id)
        {
            var brand = await _brandRepository.GetAsync(id);
            return OperationResult<BrandDto>.SuccessResult(_mapper.MapToBrandDto(brand));
        }

        public async Task<OperationResult> UpdateAsync(BrandDto entity)
        {
            var brand = await _brandRepository.GetAsync(entity.Id);
            if (brand is null)
            {
                return OperationResult.FailureResult($"Brand with id={entity.Id} does not exists");
            }
                
            await _brandRepository.UpdateAsync(_mapper.MapToBrand(entity));
            return OperationResult.SuccessResult();
        }
    }
}
