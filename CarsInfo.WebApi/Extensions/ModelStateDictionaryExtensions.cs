using System.Collections.Generic;
using System.Linq;
using CarsInfo.WebApi.ViewModels.Error;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CarsInfo.WebApi.Extensions
{
    public static class ModelStateDictionaryExtensions
    {
        public static IEnumerable<ErrorModel> GetErrorModels(this ModelStateDictionary modelState)
        {
            return modelState
                .Where(x => x.Value.Errors.Any())
                .ToDictionary(
                    x => x.Key,
                    x => x.Value.Errors.Select(e => e.ErrorMessage))
                .ToArray()
                .SelectMany(modelStateError => modelStateError.Value
                    .Select(value => new ErrorModel
                    {
                        Field = modelStateError.Key,
                        Error = value
                    }));
        }
    }
}
