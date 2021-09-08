using CarsInfo.Application.BusinessLogic.Dtos;
using CarsInfo.WebApi.ViewModels;
using CarsInfo.WebApi.ViewModels.Brand;

namespace CarsInfo.WebApi.Mappers
{
    public class BrandControllerMapper
    {
        public BrandDto MapToBrandDto(BrandEditorViewModel brandEditorViewModel)
        {
            return new BrandDto
            {
                Name = brandEditorViewModel.Name
            };
        }
    }
}