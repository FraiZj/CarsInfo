using CarsInfo.Application.BusinessLogic.Dtos;
using CarsInfo.Application.Persistence.Filters;

namespace CarsInfo.Application.BusinessLogic.Contracts
{
    public interface IFilterService
    {
        FilterModel ConfigureCarFilter(FilterDto filter);
    }
}
