using System.Collections.Generic;
using CarsInfo.BLL.Models.Dtos;
using CarsInfo.DAL.Assistance;

namespace CarsInfo.BLL.Contracts
{
    public interface IFilterService
    {
        IList<FilterModel> ConfigureCarFilter(FilterDto filter);
    }
}
