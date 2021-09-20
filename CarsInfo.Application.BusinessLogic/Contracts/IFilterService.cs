using CarsInfo.Application.BusinessLogic.Dtos;
using CarsInfo.Application.Persistence.Filters;

namespace CarsInfo.Application.BusinessLogic.Contracts
{
    public interface IFilterService
    {
        FilterModel ConfigureCarFilter(CarFilterDto carFilter);

        FilterModel ConfigureCommentFilter(int carId, CommentFilterDto filter);
    }
}
