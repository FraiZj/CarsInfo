using CarsInfo.Application.BusinessLogic.Dtos;
using CarsInfo.WebApi.ViewModels;

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